using Microsoft.EntityFrameworkCore;
using WebTorrent.Data.Models;
using WebTorrent.Data.Repositories.Interfaces;

namespace WebTorrent.Data.Repositories
{
    public class OrdersRepository : Repository<Order>, IOrdersRepository
    {
        public OrdersRepository(DbContext context) : base(context)
        {
        }


        private ApplicationDbContext _appContext => (ApplicationDbContext) _context;
    }
}