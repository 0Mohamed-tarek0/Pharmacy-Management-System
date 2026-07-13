using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.DTOs.Medicines;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Enums;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class MedicineController : Controller
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public async Task<IActionResult> Index()
        {
            var medicines = await _medicineService.GetAllAsync();

            return View(medicines);
        }

        public async Task<IActionResult> ExpiryReport()
        {
            var batches = await _medicineService.GetBatchesByExpiryAsync();

            return View(batches);
        }

        public async Task<IActionResult> Details(int id)
        {
            var medicine = await _medicineService.GetDetailsAsync(id);

            if (medicine == null)
                return NotFound();

            return View(medicine);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _medicineService.GetCategoriesAsync();

            ViewBag.Types = Enum.GetValues(typeof(MedicineType));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMedicineDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _medicineService.GetCategoriesAsync();
                ViewBag.Types = Enum.GetValues(typeof(MedicineType));

                return View(dto);
            }

            await _medicineService.CreateAsync(dto);

            TempData["Success"] = "Medicine created successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var medicine = await _medicineService.GetByIdAsync(id);

            if (medicine == null)
                return NotFound();

            ViewBag.Categories = await _medicineService.GetCategoriesAsync();

            return View(medicine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMedicineDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _medicineService.GetCategoriesAsync();
                return View(dto);
            }

            await _medicineService.UpdateAsync(dto);

            TempData["Success"] = "Medicine updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var medicine = await _medicineService.GetDetailsAsync(id);

            if (medicine == null)
                return NotFound();

            return View(medicine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _medicineService.DeleteAsync(id);

                TempData["Success"] = "Medicine deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBatch(int medicineId, int batchId)
        {
            try
            {
                await _medicineService.DeleteBatchAsync(medicineId, batchId);

                TempData["Success"] = "Batch deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Delete), new { id = medicineId });
        }


    }
}
