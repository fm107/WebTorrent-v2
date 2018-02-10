using System.IO;
using System.Threading.Tasks;
using WebTorrent.Data.Models;

namespace WebTorrent.Services.TorrentService
{
    public interface ITorrentService
    {
        Task<Content> AddTorrent(Stream file, string path);
        Task<Content> AddTorrentByUrl(string url, string path);
        Task<string> DeleteTorrent(string hash);
        Task<bool> IsTorrentType(Stream file);
        Task<Content> GetTorrentStatus(string hash);
        Task<TorrentInfo> GetTorrentDetails(string hash);
        Task<string> GetTorrentInfo(string hash);
    }
}