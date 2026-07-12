using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Infrastructure.Data;
using myshop.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Infrastructure.Repositories.Repository
{
    public class ProductRepository :BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();

        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Product?> GetWithCategoryAsync(int id)
        {
            return await _context.Products
               .Include(p => p.Category)
               .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
