using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Entities.ViewModels;
using myshop.Infrastructure.Data;
using myshop.Infrastructure.Repositories.IRepository;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace myshop.Web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
       private readonly IUnitOfWork _unitOfWork;
       private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var products = await _unitOfWork.Product.GetAllWithCategoryAsync();

            var data = products.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                description = x.Description,
                price = x.Price,
                categoryName = x.Category?.Name
            });

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _unitOfWork.Product.GetCategoriesAsync();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            }; 
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM productVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();

                    var upload = Path.Combine(rootPath, @"Images\Products");

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }

                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(
                        Path.Combine(upload, filename + extension),
                        FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    productVM.Product.Img = @"Images\Products\" + filename + extension;
                }

                await _unitOfWork.Product.AddAsync(productVM.Product);
               await _unitOfWork.SaveAsync();
               
                TempData["Create"] = "Item has Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            var categories = await _unitOfWork.Product.GetCategoriesAsync();

            productVM.CategoryList = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(productVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = await _unitOfWork.Product.GetByIdAsync(id.Value);

            if (product == null)
                return NotFound();

            var categories = await _unitOfWork.Product.GetCategoriesAsync();

            ProductVM productVM = new ProductVM()
            {

                Product = product,
                CategoryList = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            return View(productVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;

                var productFromDb = await _unitOfWork.Product.GetByIdAsync(productVM.Product.Id);

                if (productFromDb == null)
                    return NotFound();

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();

                    var upload = Path.Combine(rootPath, @"Images\Products");

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }

                    var extension = Path.GetExtension(file.FileName);

                    // Delete old image
                    if (!string.IsNullOrEmpty(productFromDb.Img))
                    {
                        var oldImage = Path.Combine(rootPath, productFromDb.Img.Replace("/", Path.DirectorySeparatorChar.ToString()));

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var fileStream = new FileStream(
                        Path.Combine(upload, filename + extension),
                        FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    productVM.Product.Img =
    @"Images\Products\" + filename + extension;
                }
                else
                {
                    // Keep the old image
                    productVM.Product.Img = productFromDb.Img;
                }


                productFromDb.Name = productVM.Product.Name;
                productFromDb.Description = productVM.Product.Description;
                productFromDb.Price = productVM.Product.Price;
                productFromDb.CategoryId = productVM.Product.CategoryId;

                await _unitOfWork.SaveAsync();

                TempData["Update"] = "Data has Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _unitOfWork.Product.GetCategoriesAsync();

            productVM.CategoryList = categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(productVM);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid Id" });
            }

            var product = await _unitOfWork.Product.GetByIdAsync(id.Value);

            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

             _unitOfWork.Product.Delete(product);

            if (!string.IsNullOrEmpty(product.Img))
            {
                var oldImage = Path.Combine(
    _webHostEnvironment.WebRootPath,
    product.Img.Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
            }

            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = "file has been Deleted" });
        }


    }
}
