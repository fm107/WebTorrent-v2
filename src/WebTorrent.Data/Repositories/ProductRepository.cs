using Microsoft.EntityFrameworkCore;
using WebTorrent.Data.Models;
using WebTorrent.Data.Repositories.Interfaces;

namespace WebTorrent.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }


        private ApplicationDbContext _appContext => (ApplicationDbContext) _context;
    }
}