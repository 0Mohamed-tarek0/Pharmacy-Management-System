using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.ViewModels.Shifts;
using PharmacyBL.DTOs.Shifts;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class ShiftController : Controller
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService) => _shiftService = shiftService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dashboard = await _shiftService.GetDashboardAsync(CurrentUserId(), User.IsInRole("Admin"));
            return View(dashboard);
        }

        [HttpGet]
        public async Task<IActionResult> Open()
        {
            var dashboard = await _shiftService.GetDashboardAsync(CurrentUserId());
            if (dashboard.OpenShift != null)
            {
                TempData["Error"] = "You already have an open shift.";
                return RedirectToAction(nameof(Index));
            }
            return View(new OpenShiftViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Open(OpenShiftViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var opened = await _shiftService.OpenAsync(new OpenShiftDto
            {
                ApplicationUserId = CurrentUserId(),
                OpeningCash = model.OpeningCash
            });
            if (!opened)
            {
                ModelState.AddModelError("", "A shift could not be opened. You may already have one open.");
                return View(model);
            }

            TempData["Success"] = "Shift opened successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Close(int id)
        {
            var shift = await _shiftService.GetByIdAsync(id, CurrentUserId());
            if (shift == null || !shift.IsOpen)
                return NotFound();

            return View(new CloseShiftViewModel { Id = id, Shift = shift });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(CloseShiftViewModel model)
        {
            var shift = await _shiftService.GetByIdAsync(model.Id, CurrentUserId());
            if (shift == null || !shift.IsOpen)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Shift = shift;
                return View(model);
            }

            var closed = await _shiftService.CloseAsync(new CloseShiftDto
            {
                Id = model.Id,
                ApplicationUserId = CurrentUserId(),
                ActualCash = model.ActualCash
            });
            if (closed == null)
            {
                TempData["Error"] = "This shift could not be closed. Please try again.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = $"Shift closed. {(closed.CashDifference >= 0 ? "Over" : "Short")} by {Math.Abs(closed.CashDifference ?? 0m):N2}.";
            return RedirectToAction(nameof(Details), new { id = closed.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var shift = await _shiftService.GetByIdAsync(id, CurrentUserId(), User.IsInRole("Admin"));
            return shift == null ? NotFound() : View(shift);
        }

        private string CurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}
