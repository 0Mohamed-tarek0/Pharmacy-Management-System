using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Sales;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _saleService.GetAllAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var sale = await _saleService.GetDetailsAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpGet("create-options")]
        public async Task<IActionResult> GetCreateOptions()
        {
            var medicines = await _saleService.GetMedicinesAsync();
            return Ok(new { medicines });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSaleDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                ModelState.AddModelError(string.Empty, "Add at least one item to the sale before submitting.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            try
            {
                var saleId = await _saleService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetDetails), new { id = saleId }, new { id = saleId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("medicine-units/{medicineId}")]
        public async Task<IActionResult> GetMedicineUnits(int medicineId)
        {
            var units = await _saleService.GetMedicineUnitsAsync(medicineId);
            return Ok(units);
        }

        [HttpPost("return-item")]
        public async Task<IActionResult> ReturnItem(ReturnSaleItemDto dto)
        {
            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _saleService.ReturnItemAsync(dto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
