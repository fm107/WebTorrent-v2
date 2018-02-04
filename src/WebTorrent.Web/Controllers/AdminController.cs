using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebTorrent.Web.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private const string UploadFolder = "uploads";
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<AdminController> _log;

        public AdminController(ILogger<AdminController> log, IHostingEnvironment environment)
        {
            _log = log;
            _environment = environment;
        }

        [HttpGet("[action]")]
        public string ShowDirectory()
        {
            string ret = null;
            foreach (var file in Directory.EnumerateFiles(Path.Combine(_environment.WebRootPath, UploadFolder), "*",
                SearchOption.AllDirectories))
            {
                ret += string.Format("{0} Size: {1}", file, new FileInfo(file).Length) + Environment.NewLine;
            }

            return ret;
        }

        [HttpGet("[action]")]
        public IActionResult RunUTorrent()
        {
            var processInfo = new ProcessStartInfo("/app/utorrent-server/utserver")
            {
                Arguments =
                    "-configfile /app/utorrent-server/utserver.conf -logfile /app/heroku_output/wwwroot/uploads/log.txt -daemon",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };

            Process.Start(processInfo)?.StandardOutput.ReadToEndAsync()
                .ContinueWith(response => { _log.LogInformation(response.Result); });

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ShowProcess()
        {
            var processInfo = new ProcessStartInfo("ps")
            {
                Arguments = "-ef",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };

            var process = Process.Start(processInfo);
            process.WaitForExit();

            return Ok(await process.StandardOutput.ReadToEndAsync());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TorrentAdmin()
        {
            FileStreamResult fileStream = null;
            const string url = "http://localhost:8080/gui/";

            var httpClientHandler = new HttpClientHandler
            {
                Credentials = new NetworkCredential("admin", ""),
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };

            var webRequest = new HttpClient(httpClientHandler);

            try
            {
                fileStream = new FileStreamResult(await webRequest.GetStreamAsync(url),
                    new MediaTypeHeaderValue("text/html"));
            }
            catch (Exception e)
            {
                _log.LogError(new EventId(1), e, e.Message);
            }

            return fileStream;
        }

        // GET: api/values
        [HttpGet]
        [Produces("text/html")]
        public string Get()
        {
            var content = new[]
            {
                @"<a href=""/api/Admin/ShowDirectory"">Show Directory</a>",
                @"<a href=""/api/Admin/ShowProcess"">Show ShowProcess</a>",
                @"<a href=""/api/Admin/RunUTorrent"">Run UTorrent</a>",
                @"<a href=""/api/Admin/TorrentAdmin"">Torrent Admin UI</a>"
            };

            var html = new StringBuilder();
            foreach (var c in content)
            {
                html.AppendLine(c);
                html.AppendLine("<br/>");
            }

            return html.ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}