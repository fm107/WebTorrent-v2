using System.Collections.Generic;
using System.Threading.Tasks;
using WebTorrent.Model;

namespace WebTorrent.Data.Repositories.Interfaces
{
    public interface IContentRepository : IRepository<Content>
    {
        Task<IList<Content>> FindByFolder(string folder, bool needFiles, string hash);
        Task<Content> FindByHash(string hash, bool tracking, string include = null);
        void Delete(params Content[] contentRecord);
        void Delete(params FileSystemItem[] fsItemsRecord);
        Task Save();
    }
}