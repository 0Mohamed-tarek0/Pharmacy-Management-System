using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Sales;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _saleService.GetAllAsync();
            return View(sales);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _saleService.GetDetailsAsync(id);
            if (sale == null)
                return NotFound();

            return View(sale);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Medicines = await _saleService.GetMedicinesAsync();
            return View(new CreateSaleDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSaleDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                ModelState.AddModelError(string.Empty, "Add at least one item to the sale before submitting.");

            if (!ModelState.IsValid)
            {
                ViewBag.Medicines = await _saleService.GetMedicinesAsync();
                return View(dto);
            }

            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            try
            {
                await _saleService.CreateAsync(dto);
                TempData["Success"] = "Sale completed successfully and stock has been updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Medicines = await _saleService.GetMedicinesAsync();
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicineUnits(int medicineId)
        {
            var units = await _saleService.GetMedicineUnitsAsync(medicineId);
            return Json(units);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnItem(ReturnSaleItemDto dto)
        {
            dto.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var result = await _saleService.ReturnItemAsync(dto);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Details), new { id = dto.SaleId });
        }
    }
}
