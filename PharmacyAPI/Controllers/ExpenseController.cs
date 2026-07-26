using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Expenses;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseCategoryService _expenseCategoryService;

        public ExpenseController(IExpenseService expenseService, IExpenseCategoryService expenseCategoryService)
        {
            _expenseService = expenseService;
            _expenseCategoryService = expenseCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _expenseService.GetAllAsync();
            return Ok(expenses);
        }

        // GET api/expense/create-options
        // Replaces the ViewBag.ExpenseCategories SelectList built by PopulateCategoriesAsync in MVC.
        [HttpGet("create-options")]
        public async Task<IActionResult> GetCreateOptions()
        {
            var categories = await _expenseCategoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            bool result = await _expenseService.CreateAsync(dto);
            if (!result)
                return BadRequest(new { message = "Unable to create expense. Please verify the category and try again." });

            return StatusCode(StatusCodes.Status201Created, dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateExpenseDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Route id and body id do not match." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _expenseService.UpdateAsync(dto);
            if (!result)
                return BadRequest(new { message = "Expense not found or category is invalid." });

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _expenseService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Expense not found or could not be deleted." });

            return NoContent();
        }
    }
}
