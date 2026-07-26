using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Categories;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET api/category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET api/category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST api/category
        // MVC took a CreateCategoryViewModel from a form and mapped it to a
        // CreateCategoryDto itself. The API skips that step and binds the
        // DTO directly from the JSON body, since there's no Razor form involved.
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _categoryService.CreateAsync(dto);

            if (!result)
                return Conflict(new { message = "Category already exists." });

            return CreatedAtAction(nameof(GetById), new { id = dto.Name }, dto);
        }

        // PUT api/category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Route id and body id do not match." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _categoryService.UpdateAsync(dto);

            if (!result)
                return Conflict(new { message = "Category name already exists." });

            return NoContent();
        }

        // DELETE api/category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _categoryService.DeleteAsync(id);

            if (!result)
                return Conflict(new { message = "Cannot delete this category because it contains medicines." });

            return NoContent();
        }
    }
}
