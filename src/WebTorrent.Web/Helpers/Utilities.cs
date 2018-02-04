using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.Extensions.Logging;

namespace WebTorrent.Web.Helpers
{
    public static class Utilities
    {
        private static ILoggerFactory _loggerFactory;


        public static void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }


        public static ILogger CreateLogger<T>()
        {
            if (_loggerFactory == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(ILogger)} is not configured. {nameof(ConfigureLogger)} must be called before use");
            }

            return _loggerFactory.CreateLogger<T>();
        }


        public static void QuickLog(string text, string filename)
        {
            var dirPath = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (var writer = File.AppendText(filename))
            {
                writer.WriteLine($"{DateTime.Now} - {text}");
            }
        }


        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }


        public static string[] GetRoles(ClaimsPrincipal identity)
        {
            return identity.Claims
                .Where(c => c.Type == OpenIdConnectConstants.Claims.Role)
                .Select(c => c.Value)
                .ToArray();
        }
    }
}