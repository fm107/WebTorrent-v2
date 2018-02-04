using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebTorrent.Tools.FFmpeg
{
    public class FFmpeg
    {
        private readonly TaskFactory _factory;
        private readonly FFmpegSettings _ffmpegSettings;
        private readonly ILogger<FFmpeg> _log;
        private FFmpegArguments _ffmpeg;

        public FFmpeg(IOptions<FFmpegSettings> ffmpegOptions, ILogger<FFmpeg> log)
        {
            _log = log;
            _ffmpegSettings = ffmpegOptions.Value;

            _factory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(4));
        }

        public void CreatePlayList(string fileToConvert, string outputPath, string playList)
        {
            _factory.StartNew(() =>
            {
                _ffmpeg = new FFmpegBuilder(_ffmpegSettings.FilePath, fileToConvert)
                    .MapVideoStream(1)
                    .MapAudioStream(1)
                    .CreateOnlinePlayList(outputPath, playList)
                    .Build();

                var processInfo = new ProcessStartInfo(_ffmpegSettings.FilePath)
                {
                    Arguments = _ffmpeg.CmdArguments,
                    RedirectStandardOutput = true
                };

                using (var process = Process.Start(processInfo))
                {
                    process.WaitForExit();
                    var output = process.StandardOutput.ReadToEnd();
                    _log.LogInformation(output);
                }
            });
        }
    }
}
