using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTorrent.Data;
using WebTorrent.Data.Repositories.Interfaces;
using WebTorrent.Model;

namespace WebTorrent.Repository
{
    internal class ContentRepository : BaseRepository<ContentDbContext, Content>, IContentRepository
    {
        private readonly ContentDbContext _context;

        public ContentRepository(ContentDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<Content>> FindByFolder(string folder, bool needFiles, string hash)
        {
            if (needFiles)
            {
                var contentbyHash = await FindByHash(hash, false, "FsItems");
                contentbyHash.FsItems = contentbyHash.FsItems.Where(b => b.Type.Equals("file")).ToList();
                return new[] {contentbyHash};
            }

            var contents = await _context.Content.Where(t => t.ParentFolder.Equals(folder)).Include(f => f.FsItems)
                .AsNoTracking().ToListAsync();

            foreach (var content in contents)
            {
                content.FsItems = content.FsItems.Where(b => b.Type.Equals("folder")).ToList();
            }

            return contents;
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public async Task<Content> FindByHash(string hash, bool tracking, string include = null)
        {
            if (!string.IsNullOrEmpty(include) && tracking)
            {
                return await _context.Content.Include(include).FirstOrDefaultAsync(t => t.Hash.Equals(hash));
            }

            if (!string.IsNullOrEmpty(include) && !tracking)
            {
                return await _context.Content.Include(include).AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Hash.Equals(hash));
            }

            if (string.IsNullOrEmpty(include) && tracking)
            {
                return await _context.Content.FirstOrDefaultAsync(t => t.Hash.Equals(hash));
            }

            if (string.IsNullOrEmpty(include) && !tracking)
            {
                return await _context.Content.AsNoTracking().FirstOrDefaultAsync(t => t.Hash.Equals(hash));
            }

            return await Task.FromResult<Content>(null);
        }

        public void Delete(params Content[] contentRecord)
        {
            _context.Content.RemoveRange(contentRecord);
        }

        public void Delete(params FileSystemItem[] fsItemsRecord)
        {
            _context.FsItem.RemoveRange(fsItemsRecord);
        }

        async Task IContentRepository.Save()
        {
            await _context.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<Content> entities)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Content> entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(Content entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Content> entities)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Content> Find(Expression<Func<Content, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Content GetSingleOrDefault(Expression<Func<Content, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Content Get(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Content> IRepository<Content>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}