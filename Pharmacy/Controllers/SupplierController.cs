using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.ViewModels.Suppliers;
using PharmacyBL.DTOs.Suppliers;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllAsync();

            return View(suppliers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplierViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreateSupplierDto
            {
                CompanyName = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email
            };

            bool result = await _supplierService.CreateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Supplier already exists.");
                return View(model);
            }
            TempData["Success"] = "Supplier created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null)
                return NotFound();
            var model = new UpdateSupplierViewModel
            {
                Id = supplier.Id,
                CompanyName = supplier.CompanyName,
                Address = supplier.Address,
                Phone = supplier.Phone,
                Email = supplier.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSupplierViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var dto = new UpdateSupplierDto
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email
            };
            bool result = await _supplierService.UpdateAsync(dto);
            if (!result)
            {
                ModelState.AddModelError("", "Supplier already exists.");
                return View(model);
            }
            TempData["Success"] = "Supplier updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _supplierService.DeleteAsync(id);

            if (!result)
            {
                TempData["Error"] =
                    "Supplier cannot be deleted because it has related records.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] =
                "Supplier deleted successfully.";

            return RedirectToAction(nameof(Index));
        }


    }
}

