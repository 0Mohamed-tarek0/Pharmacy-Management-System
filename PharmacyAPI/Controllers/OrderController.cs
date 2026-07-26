using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Orders;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var order = await _orderService.GetDetailsAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // GET api/order/create-options
        // Replaces ViewBag.Suppliers / ViewBag.Medicines from the MVC Create() GET.
        [HttpGet("create-options")]
        public async Task<IActionResult> GetCreateOptions()
        {
            var suppliers = await _orderService.GetSuppliersAsync();
            var medicines = await _orderService.GetMedicinesAsync();
            return Ok(new { suppliers, medicines });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                ModelState.AddModelError(string.Empty, "Add at least one item to the order before submitting.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Same as MVC: the current user comes from the auth token's claims,
            // not from a hidden form field, so it can't be spoofed by the client.
            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            if (dto.SupplierId == 0)
                return BadRequest(new { message = "Please select a supplier." });

            try
            {
                var orderId = await _orderService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetDetails), new { id = orderId }, new { id = orderId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/order/medicine-units/5
        [HttpGet("medicine-units/{medicineId}")]
        public async Task<IActionResult> GetMedicineUnits(int medicineId)
        {
            var units = await _orderService.GetMedicineUnitsAsync(medicineId);
            return Ok(units);
        }

        [HttpPost("return-item")]
        public async Task<IActionResult> ReturnItem(ReturnOrderItemDto dto)
        {
            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _orderService.ReturnItemAsync(dto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
