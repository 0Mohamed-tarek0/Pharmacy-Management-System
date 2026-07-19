using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pharmacy.ViewModels.Expenses;
using PharmacyBL.DTOs.Expenses;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseCategoryService _expenseCategoryService;

        public ExpenseController(
            IExpenseService expenseService,
            IExpenseCategoryService expenseCategoryService)
        {
            _expenseService = expenseService;
            _expenseCategoryService = expenseCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllAsync();
            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesAsync();
            return View(new CreateExpenseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync(model.ExpenseCategoryId);
                return View(model);
            }

            var dto = new CreateExpenseDto
            {
                Title = model.Title,
                Amount = model.Amount,
                ExpenseCategoryId = model.ExpenseCategoryId,
                ExpenseDate = model.ExpenseDate,
                Notes = model.Notes,
                ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty
            };

            bool result = await _expenseService.CreateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Unable to create expense. Please verify the category and try again.");
                await PopulateCategoriesAsync(model.ExpenseCategoryId);
                return View(model);
            }

            TempData["Success"] = "Expense recorded successfully.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);

            if (expense == null)
                return NotFound();

            var model = new UpdateExpenseViewModel
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount = expense.Amount,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                ExpenseDate = expense.ExpenseDate,
                Notes = expense.Notes
            };

            await PopulateCategoriesAsync(model.ExpenseCategoryId);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync(model.ExpenseCategoryId);
                return View(model);
            }

            var dto = new UpdateExpenseDto
            {
                Id = model.Id,
                Title = model.Title,
                Amount = model.Amount,
                ExpenseCategoryId = model.ExpenseCategoryId,
                ExpenseDate = model.ExpenseDate,
                Notes = model.Notes
            };

            bool result = await _expenseService.UpdateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Expense not found or category is invalid.");
                await PopulateCategoriesAsync(model.ExpenseCategoryId);
                return View(model);
            }

            TempData["Success"] = "Expense updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _expenseService.DeleteAsync(id);

            if (!result)
            {
                TempData["Error"] = "Expense not found or could not be deleted.";
            }
            else
            {
                TempData["Success"] = "Expense deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCategoriesAsync(int? selectedCategoryId = null)
        {
            var categories = await _expenseCategoryService.GetAllAsync();

            ViewBag.ExpenseCategories = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }
    }
}
