using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.ViewModels.Dashboard;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IFinancialService _financialService;
        private readonly IMedicineService _medicineService;
        private readonly IShiftService _shiftService;
        private readonly ISaleService _saleService;

        public DashboardController(IFinancialService financialService, IMedicineService medicineService,
            IShiftService shiftService, ISaleService saleService)
        {
            _financialService = financialService;
            _medicineService = medicineService;
            _shiftService = shiftService;
            _saleService = saleService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var isAdmin = User.IsInRole("Admin");
            var todayStart = DateTime.UtcNow.Date;
            var todayEnd = todayStart.AddDays(1).AddTicks(-1);
            var medicines = (await _medicineService.GetAllAsync()).ToList();
            var shifts = await _shiftService.GetDashboardAsync(userId);
            var financial = isAdmin ? await _financialService.GetDailySummaryAsync() : null;

            return View(new DashboardViewModel
            {
                IsAdmin = isAdmin,
                TodaySales = financial?.Sales ?? await _saleService.GetTotalByUserAndDateRangeAsync(userId, todayStart, todayEnd),
                TodayNetProfit = financial?.NetProfit ?? 0m,
                TodayExpenses = financial?.Expenses ?? 0m,
                MedicineCount = medicines.Count,
                LowStockCount = medicines.Count(m => m.IsLowStock),
                LowStockMedicines = medicines.Where(m => m.IsLowStock).OrderBy(m => m.TotalQuantity).Take(5),
                OpenShift = shifts.OpenShift
            });
        }
    }
}
