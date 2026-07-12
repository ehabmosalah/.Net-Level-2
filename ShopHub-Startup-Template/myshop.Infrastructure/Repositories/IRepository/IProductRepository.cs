using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Infrastructure.Repositories.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        Task<Product?> GetWithCategoryAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}
