using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using UTorrent.Api;
using UTorrent.Api.Data;
using WebTorrent.Common.MimeTypes;
using WebTorrent.Data.Models;
using WebTorrent.Data.Repositories.Interfaces;
using WebTorrent.Services.FileSystemService;
using WebTorrent.Tools.FFmpeg;
using File = System.IO.File;

namespace WebTorrent.Services.TorrentService
{
    public class TorrentService : ITorrentService
    {
        private readonly UTorrentClient _client;
        private readonly IHostingEnvironment _environment;
        private readonly FFmpeg _ffmpeg;
        private readonly FsInfoService _fsInfo;
        private readonly ILogger<TorrentService> _log;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IContentRepository _repository;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Timer _timer;

        public TorrentService(FsInfoService fsInfo, IContentRepository repository, ILogger<TorrentService> log,
            FFmpeg ffmpeg, IHostingEnvironment environment, IMapper mapper, IMemoryCache memoryCache)
        {
            _fsInfo = fsInfo;
            _repository = repository;
            _log = log;
            _ffmpeg = ffmpeg;
            _environment = environment;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _client = new UTorrentClient("admin", "");
        }

        public async Task<Content> AddTorrent(Stream file, string path)
        {
            var response = _client.PostTorrent(file, Path.Combine(path, await GetTorrentName(file)));
            var torrent = response.AddedTorrent;

            await StartTimer(CheckStatus, (int) TimeSpan.FromSeconds(10).TotalMilliseconds);
            return await _fsInfo.SaveFolderContent(torrent, await GetFiles(torrent.Hash));
        }

        public async Task<Content> AddTorrentByUrl(string url, string path)
        {
            var response = _client.AddUrlTorrent(url, Path.Combine(path, GetTorrentName(url)));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Torrent torrent;

            do
            {
                if (stopwatch.Elapsed < TimeSpan.FromMinutes(3))
                {
                    torrent = (await _client.GetListAsync()).Result.Torrents.FirstOrDefault(
                        t => t.Hash.Equals(response.AddedTorrent.Hash));
                    Thread.Sleep(1000);
                }
                else
                {
                    return null;
                }
            } while (torrent.Size <= 0);

            await StartTimer(CheckStatus, (int) TimeSpan.FromSeconds(10).TotalMilliseconds);
            return await _fsInfo.SaveFolderContent(torrent, await GetFiles(response.AddedTorrent.Hash));
        }

        public async Task<string> DeleteTorrent(string hash)
        {
            try
            {
                await _semaphore.WaitAsync();

                var contentbyHash = await _repository.FindByHash(hash, true, "FsItems");

                var response = await _client.DeleteTorrentAsync(hash);
                if (response.Error != null)
                {
                    return response.Error.Message;
                }

                _repository.Delete(contentbyHash);
                _repository.Delete(contentbyHash.FsItems.ToArray());

                await _repository.Save();

#pragma warning disable 4014
                Task.Run(() => DeleteDirectory(contentbyHash.FsItems.FirstOrDefault()?.FullName));
#pragma warning restore 4014

                return contentbyHash.TorrentName;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static async Task DeleteDirectory(string directoryPath, int maxRetries = 10, int millisecondsDelay = 30)
        {
            var files = Directory.GetFiles(directoryPath);
            var dirs = Directory.GetDirectories(directoryPath);

            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var dir in dirs)
            {
                await DeleteDirectory(dir);
            }

            for (var i = 0; i < maxRetries; ++i)
            {
                try
                {
                    Directory.Delete(directoryPath, false);
                }
                catch (IOException)
                {
                    await Task.Delay(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    await Task.Delay(millisecondsDelay);
                }
            }
        }

        public async Task<bool> IsTorrentType(Stream file)
        {
            var buffer = new byte[11];
            var torrentType = new byte[] {0x64, 0x38, 0x3a, 0x61, 0x6e, 0x6e, 0x6f, 0x75, 0x6e, 0x63, 0x65};

            if (file.CanRead)
            {
                await file.ReadAsync(buffer, 0, buffer.Length);
                if (file.CanSeek)
                {
                    file.Seek(0, SeekOrigin.Begin);
                }
            }

            return buffer.SequenceEqual(torrentType);
        }

        public async Task<Content> GetTorrentStatus(string hash)
        {
            object content;
            var isExist = _memoryCache.TryGetValue(string.Format("{0}-TorrentStatus", hash), out content);

            if (isExist) return (Content) content;

            try
            {
                await _semaphore.WaitAsync();

                content = await _repository.FindByHash(hash, false);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _memoryCache.Set(hash, content, cacheEntryOptions);

                return (Content) content;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<TorrentInfo> GetTorrentDetails(string hash)
        {
            object status;
            var isExist = _memoryCache.TryGetValue(string.Format("{0}-TorrentDetails", hash), out status);

            if (isExist) return (TorrentInfo) status;

            var torrent = await _client.GetTorrentAsync(hash);

            status = _mapper.Map<TorrentInfo>(torrent.Result.Torrents.FirstOrDefault(t => t.Hash.Equals(hash))) ??
                     new TorrentInfo();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));

            _memoryCache.Set(hash, status, cacheEntryOptions);

            return (TorrentInfo) status;
        }

        public async Task<string> GetTorrentInfo(string hash)
        {
            var torrent = await _client.GetTorrentAsync(hash);
            string result = null;
            foreach (var tor in torrent.Result.Torrents.Where(t => t.Hash.Equals(hash)))
            {
                result = $"Progress: {tor.Progress / 10.0}%" + Environment.NewLine +
                         $"Remaining: {tor.Remaining} bytes";
            }

            return result;
        }

        private async Task StartTimer(TimerCallback callback, int period)
        {
            if (_timer == null && (await _client.GetListAsync()).Result.Torrents.Any(t => t.Progress != 1000))
            {
                _timer = new Timer(callback, null, 0, period);
            }
        }

        private void StopTimer()
        {
            _timer.Dispose();
            _timer = null;
        }

        private async void CheckStatus(object state)
        {
            try
            {
                await _semaphore.WaitAsync();

                var torrents = (await _client.GetListAsync()).Result.Torrents;

                foreach (var tor in torrents)
                {
                    if (tor.Progress != 1000)
                    {
                        continue;
                    }

                    var content = await _repository.FindByHash(tor.Hash, true);

                    if (content?.IsInProgress != true)
                    {
                        continue;
                    }

                    _log.LogInformation("Creating playing list for {0}", tor.Name);

                    ChangeStatus(content);
                    CreatePlayList(content);
                    ValidateStateTorrents(torrents);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private void ValidateStateTorrents(IEnumerable<UTorrent.Api.Data.Torrent> torrents)
        {
            if (_timer != null && torrents.All(t => t.Progress == 1000))
            {
                StopTimer();
            }
        }

        private void CreatePlayList(Content content)
        {
            foreach (var file in content.FsItems.Where(f => f.Type.Equals("file")))
            {
                if (MimeTypes.GetMimeMapping(file.Name).Contains("video") |
                    MimeTypes.GetMimeMapping(file.Name).Contains("audio"))
                {
                    var fileToConvert = Path.Combine(file.FullName, file.Name);

                    _log.LogInformation("Start convert process for {0}", fileToConvert);
                    _log.LogInformation("file path {0}", file.FullName);

                    file.Stream = Path.Combine(file.FullName.Replace(_environment.WebRootPath, string.Empty),
                        file.Name + ".m3u8");
                    file.IsStreaming = true;
                    _repository.Update(content);
                    _repository.Save();

                    _ffmpeg.CreatePlayList(fileToConvert, file.FullName, file.Name);
                }
            }
        }

        private void ChangeStatus(Content content)
        {
            content.IsInProgress = false;

            _repository.Update(content);
            _repository.Save();
        }

        private async Task<ICollection<FileCollection>> GetFiles(string hash)
        {
            var response = await _client.GetFilesAsync(hash);
            return response.Result.Files.Values;
        }

        private static string GetTorrentName(string file)
        {
            var torrentName = file.ToCharArray(20, 40);
            return string.Concat(torrentName);
        }

        private static async Task<string> GetTorrentName(Stream file)
        {
            var buffer = new byte[file.Length];
            var regex = new Regex(":name[0-9]{2}:(.*?)[0-9]{2}:piece");

            if (file.CanRead)
            {
                await file.ReadAsync(buffer, 0, buffer.Length);
                if (file.CanSeek)
                {
                    file.Seek(0, SeekOrigin.Begin);
                }
            }
            var stringOfStream = Encoding.UTF8.GetString(buffer);
            return Path.ChangeExtension(regex.Match(stringOfStream).Groups[1].Value, null);
        }
    }
}