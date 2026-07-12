using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Infrastructure.Repositories.IRepository
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
       // void Delete(int? id);
    }
}
