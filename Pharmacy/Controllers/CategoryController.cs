using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.ViewModels.Categories;
using PharmacyBL.DTOs.Categories;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreateCategoryDto
            {
                Name = model.Name,
                Description = model.Description
            };

            bool result = await _categoryService.CreateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Category already exists.");

                return View(model);
            }

            TempData["Success"] = "Category created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            var model = new UpdateCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UpdateCategoryDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            bool result = await _categoryService.UpdateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Category name already exists.");
                return View(model);
            }

            TempData["Success"] = "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _categoryService.DeleteAsync(id);

            if (!result)
            {
                TempData["Error"] = "Cannot delete this category because it contains medicines.";
            }
            else
            {
                TempData["Success"] = "Category deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}