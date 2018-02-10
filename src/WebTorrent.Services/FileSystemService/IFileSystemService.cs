using System.Collections.Generic;
using System.Threading.Tasks;
using UTorrent.Api.Data;
using WebTorrent.Data.Models;

namespace WebTorrent.Services.FileSystemService
{
    public interface IFileSystemService
    {
        Task<IList<Content>> GetFolderContent(string folder, bool needFiles, string hash);

        Task<Content> SaveFolderContent(UTorrent.Api.Data.Torrent torrent,
            ICollection<FileCollection> collection);
    }
}