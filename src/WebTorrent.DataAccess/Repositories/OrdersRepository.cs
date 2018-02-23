using WebTorrent.Data.Models;
using WebTorrent.Data.Repositories.Interfaces;

namespace WebTorrent.Data.Repositories
{
    public class OrdersRepository : BaseRepository<ApplicationDbContext, Order>, IOrdersRepository
    {
        public OrdersRepository(ApplicationDbContext context) : base(context)
        {
        }


        private ApplicationDbContext _appContext => _context;
    }
}