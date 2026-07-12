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
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        //public void Delete(int? id)
        //{
        //   _context.Categories.Remove(_context.Categories.fin(id));
        //}
    }
}
