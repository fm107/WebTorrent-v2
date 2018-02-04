using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebTorrent.Tools.FFmpeg
{
    public class FFmpegBuilder
    {
        private readonly StringBuilder _builder;
        private readonly List<Action<FFmpegArguments>> _builderActions;
        private readonly string _ffmpegPath;
        private readonly string _inputFile;
        private readonly Dictionary<string, string> _audioList;
        private readonly Dictionary<string, string> _subtitleList;
        private readonly Dictionary<string, string> _videoList;

        public FFmpegBuilder(string ffmpegPath, string inputFile)
        {
            _ffmpegPath = ffmpegPath;
            _inputFile = inputFile;
            _builder = new StringBuilder(string.Format(@"-i ""{0}""", inputFile));
            _builderActions = new List<Action<FFmpegArguments>>();
            _videoList = new Dictionary<string, string>();
            _audioList = new Dictionary<string, string>();
            _subtitleList = new Dictionary<string, string>();
        }

        public FFmpegBuilder MapVideoStream(int amountStreamsToMap)
        {
            _builderActions.Add(config =>
            {
                foreach (var video in _videoList.Take(amountStreamsToMap))
                    _builder.AppendFormat(" -map {0}", video.Value);

                config.CmdArguments = _builder.ToString();
            });

            return this;
        }

        public FFmpegBuilder MapAudioStream(int amountStreamsToMap)
        {
            _builderActions.Add(config =>
            {
                foreach (var video in _audioList.Take(amountStreamsToMap))
                    _builder.AppendFormat(" -map {0}", video.Value);

                config.CmdArguments = _builder.ToString();
            });

            return this;
        }

        public FFmpegBuilder ConvertToMp4()
        {
            _builderActions.Add(config =>
            {
                _builder.AppendFormat(
                    @" -f mp4 -vcodec libx264 -preset ultrafast -movflags faststart -profile:v main -acodec aac {1} -hide_banner",
                    _inputFile, string.Format("{0}.mp4", Path.ChangeExtension(_inputFile, null)));

                config.CmdArguments = _builder.ToString();
            });

            return this;
        }

        public FFmpegBuilder CreateOnlinePlayList(string outputPath, string playList)
        {
            _builderActions.Add(config =>
            {
                var tsFileName = playList.Replace(" ", "_");
                _builder.AppendFormat(
                    @" -c:v libx264 -c:a aac -preset ultrafast -profile:v baseline -level 3.0 -threads 0 -force_key_frames ""expr:gte(t,n_forced*10)"" -f segment -segment_time 10 -segment_format mpegts -segment_list_flags +live -segment_list ""{0}/{1}.m3u8"" -segment_list_type m3u8 ""{0}/{2}.%d.ts""",
                    outputPath, playList, tsFileName);

                config.CmdArguments = _builder.ToString();
            });

            return this;
        }

        public FFmpegArguments Build()
        {
            GetStreams(_inputFile);

            var ffmpegArguments = new FFmpegArguments();
            _builderActions.ForEach(build => build(ffmpegArguments));

            return ffmpegArguments;
        }

        private void GetStreams(string fileToConvert)
        {
            var processInfo = new ProcessStartInfo(_ffmpegPath)
            {
                Arguments = string.Format(@"-i ""{0}""", fileToConvert),
                RedirectStandardError = true
            };

            var process = Process.Start(processInfo);

            process.WaitForExit();
            var output = process.StandardError.ReadToEnd();

            var streamLine = new Regex("(Stream #(.*))");
            var streamChannel = new Regex(@"(Stream #)(\d+)(\W+)(\d+)");

            foreach (Match match in streamLine.Matches(output))
            {
                if (match.Value.Contains("Video:"))
                {
                    var channel = streamChannel.Match(match.Value).Value.Split('#').Last();
                    _videoList.Add(fileToConvert + channel, channel);
                }

                if (match.Value.Contains("Audio:"))
                {
                    var channel = streamChannel.Match(match.Value).Value.Split('#').Last();
                    _audioList.Add(fileToConvert + channel, channel);
                }

                if (match.Value.Contains("Subtitle:"))
                {
                    var channel = streamChannel.Match(match.Value).Value.Split('#').Last();
                    _subtitleList.Add(fileToConvert + channel, channel);
                }
            }
        }
    }
}