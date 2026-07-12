using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Entities.DTOs;
using myshop.Infrastructure.Repositories.IRepository;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;

namespace myshop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return View(categoriesDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _unitOfWork.Category.AddAsync(category);
                await _unitOfWork.SaveAsync();
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(categoryDto);
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

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return View(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryDto);
                _unitOfWork.Category.Update(category);

                await _unitOfWork.SaveAsync();
                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(categoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
               return NotFound();
            }
            var category = await _unitOfWork.Category.GetByIdAsync(id.Value);

            if (category == null)
                return NotFound();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return View(categoryDto);
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
