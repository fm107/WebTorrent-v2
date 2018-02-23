using WebTorrent.Data.Repositories;
using WebTorrent.Data.Repositories.Interfaces;

namespace WebTorrent.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private ICustomerRepository _customers;
        private IOrdersRepository _orders;
        private IProductRepository _products;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public ICustomerRepository Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = new CustomerRepository(_context);
                }

                return _customers;
            }
        }


        public IProductRepository Products
        {
            get
            {
                if (_products == null)
                {
                    _products = new ProductRepository(_context);
                }

                return _products;
            }
        }


        public IOrdersRepository Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new OrdersRepository(_context);
                }

                return _orders;
            }
        }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}