using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.ExpenseCategories;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IExpenseCategoryService _expenseCategoryService;

        public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _expenseCategoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _expenseCategoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _expenseCategoryService.CreateAsync(dto);
            if (!result)
                return Conflict(new { message = "An expense category with this name already exists." });

            return StatusCode(StatusCodes.Status201Created, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateExpenseCategoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Route id and body id do not match." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _expenseCategoryService.UpdateAsync(dto);
            if (!result)
                return Conflict(new { message = "Expense category not found or name already exists." });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _expenseCategoryService.DeleteAsync(id);
            if (!result)
                return Conflict(new { message = "Cannot delete this category because it has related expenses." });

            return NoContent();
        }
    }
}
