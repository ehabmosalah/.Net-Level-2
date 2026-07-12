using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Infrastructure.Data;
using myshop.Infrastructure.Repositories.IRepository;
using System.Threading.Tasks;

namespace myshop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
               await _unitOfWork.Category.AddAsync(category);
               await _unitOfWork.SaveAsync();
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = await _unitOfWork.Category.GetByIdAsync(id.Value);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);

                await _unitOfWork.SaveAsync();
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
               return  NotFound();
            }
            var category = await _unitOfWork.Category.GetByIdAsync(id.Value);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = await _unitOfWork.Category.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Delete(category);
           await _unitOfWork.SaveAsync();
            TempData["Delete"] = "Item has Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
