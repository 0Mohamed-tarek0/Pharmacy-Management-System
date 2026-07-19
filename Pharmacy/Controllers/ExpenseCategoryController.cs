using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.ViewModels.ExpenseCategories;
using PharmacyBL.DTOs.ExpenseCategories;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExpenseCategoryController : Controller
    {
        private readonly IExpenseCategoryService _expenseCategoryService;

        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _expenseCategoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExpenseCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreateExpenseCategoryDto
            {
                Name = model.Name
            };

            bool result = await _expenseCategoryService.CreateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "An expense category with this name already exists.");
                return View(model);
            }

            TempData["Success"] = "Expense category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _expenseCategoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            var model = new UpdateExpenseCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateExpenseCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UpdateExpenseCategoryDto
            {
                Id = model.Id,
                Name = model.Name
            };

            bool result = await _expenseCategoryService.UpdateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Expense category not found or name already exists.");
                return View(model);
            }

            TempData["Success"] = "Expense category updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _expenseCategoryService.DeleteAsync(id);

            if (!result)
            {
                TempData["Error"] = "Cannot delete this category because it has related expenses.";
            }
            else
            {
                TempData["Success"] = "Expense category deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
