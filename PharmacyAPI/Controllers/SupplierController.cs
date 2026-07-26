using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Suppliers;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _supplierService.CreateAsync(dto);
            if (!result)
                return Conflict(new { message = "Supplier already exists." });

            return StatusCode(StatusCodes.Status201Created, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSupplierDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Route id and body id do not match." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _supplierService.UpdateAsync(dto);
            if (!result)
                return Conflict(new { message = "Supplier already exists." });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _supplierService.DeleteAsync(id);
            if (!result)
                return Conflict(new { message = "Supplier cannot be deleted because it has related records." });

            return NoContent();
        }
    }
}
