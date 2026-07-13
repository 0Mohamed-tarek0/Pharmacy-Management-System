using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Orders;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetDetailsAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = await _orderService.GetSuppliersAsync();
            ViewBag.Medicines = await _orderService.GetMedicinesAsync();

            return View(new CreateOrderDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                ModelState.AddModelError(string.Empty, "Add at least one item to the order before submitting.");

            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = await _orderService.GetSuppliersAsync();
                ViewBag.Medicines = await _orderService.GetMedicinesAsync();

                return View(dto);
            }

            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            try
            {
                if (dto.SupplierId == 0)
                {
                    ModelState.AddModelError("", "Please select a supplier.");
                }
                await _orderService.CreateAsync(dto);

                TempData["Success"] = "Order received and stock updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                ViewBag.Suppliers = await _orderService.GetSuppliersAsync();
                ViewBag.Medicines = await _orderService.GetMedicinesAsync();

                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicineUnits(int medicineId)
        {
            var units = await _orderService.GetMedicineUnitsAsync(medicineId);

            return Json(units);
        }
    }
}
