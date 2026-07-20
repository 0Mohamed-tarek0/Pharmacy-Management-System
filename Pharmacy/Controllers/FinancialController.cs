using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FinancialController : Controller
    {
        private readonly IFinancialService _financialService;

        public FinancialController(IFinancialService financialService)
        {
            _financialService = financialService;
        }

        /// <summary>
        /// Displays the daily financial summary.
        /// Accepts an optional <paramref name="date"/> query-string parameter
        /// so users can review any previous day (defaults to today).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DailySummary(DateTime? date)
        {
            var summary = await _financialService.GetDailySummaryAsync(date);
            return View(summary);
        }

        public async Task<IActionResult> OverallReport()
        {
            var model =
                await _financialService.GetOverallFinancialReportAsync();

            return View(model);
        }
    }
}
