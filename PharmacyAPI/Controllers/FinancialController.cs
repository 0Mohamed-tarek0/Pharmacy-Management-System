using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyBL.Interfaces.Services;

namespace PharmacyAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly IFinancialService _financialService;

        public FinancialController(IFinancialService financialService) => _financialService = financialService;

        // GET api/financial?period=today
        // MVC redirected to a Razor page with TempData["Error"] on a bad range;
        // the API just returns 400 with a JSON error instead of a redirect.
        [HttpGet]
        public async Task<IActionResult> Index(string period = "today", DateTime? from = null, DateTime? to = null)
        {
            var (rangeFrom, rangeTo, label) = ResolveRange(period, from, to);

            if (rangeFrom > rangeTo)
                return BadRequest(new { message = "The start date must be on or before the end date." });

            var summary = await _financialService.GetSummaryForRangeAsync(rangeFrom, rangeTo);

            return Ok(new
            {
                period,
                from = rangeFrom,
                to = rangeTo,
                periodLabel = label,
                summary
            });
        }

        [HttpGet("daily-summary")]
        public async Task<IActionResult> DailySummary(DateTime? date = null)
        {
            var summary = await _financialService.GetDailySummaryAsync(date);
            return Ok(summary);
        }

        [HttpGet("overall-report")]
        public async Task<IActionResult> OverallReport()
        {
            var report = await _financialService.GetOverallFinancialReportAsync();
            return Ok(report);
        }

        private static (DateTime From, DateTime To, string Label) ResolveRange(string? period, DateTime? from, DateTime? to)
        {
            var today = DateTime.UtcNow.Date;
            return period?.ToLowerInvariant() switch
            {
                "yesterday" => (today.AddDays(-1), today.AddDays(-1), "Yesterday"),
                "three-days-ago" => (today.AddDays(-3), today.AddDays(-3), "Three Days Ago"),
                "this-week" => (today.AddDays(-((int)today.DayOfWeek + 6) % 7), today, "This Week"),
                "last-week" => (today.AddDays(-((int)today.DayOfWeek + 6) % 7 - 7), today.AddDays(-((int)today.DayOfWeek + 6) % 7 - 1), "Last Week"),
                "last-2-weeks" => (today.AddDays(-13), today, "Last 14 Days"),
                "this-month" => (new DateTime(today.Year, today.Month, 1), today, "This Month"),
                "last-month" => (new DateTime(today.Year, today.Month, 1).AddMonths(-1), new DateTime(today.Year, today.Month, 1).AddDays(-1), "Last Month"),
                "this-year" => (new DateTime(today.Year, 1, 1), today, "This Year"),
                "last-year" => (new DateTime(today.Year - 1, 1, 1), new DateTime(today.Year - 1, 12, 31), "Last Year"),
                "all" => (new DateTime(2000, 1, 1), today, "All Recorded Time"),
                "custom" => (from?.Date ?? today, to?.Date ?? today, "Custom Range"),
                _ => (today, today, "Today")
            };
        }
    }
}
