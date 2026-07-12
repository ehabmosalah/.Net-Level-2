using myshop.Infrastructure.Data;
using myshop.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Infrastructure.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category { get; }

        public IProductRepository Product { get; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Category = new CategoryRepository(_context);
            Product = new ProductRepository(_context);
        }

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
