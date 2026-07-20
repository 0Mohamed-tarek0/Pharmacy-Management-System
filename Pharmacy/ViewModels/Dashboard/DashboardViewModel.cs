using PharmacyBL.DTOs.Medicines;
using PharmacyBL.DTOs.Shifts;

namespace Pharmacy.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public bool IsAdmin { get; set; }
        public decimal TodaySales { get; set; }
        public decimal TodayNetProfit { get; set; }
        public decimal TodayExpenses { get; set; }
        public int MedicineCount { get; set; }
        public int LowStockCount { get; set; }
        public ShiftDto? OpenShift { get; set; }
        public IEnumerable<MedicineDto> LowStockMedicines { get; set; } = Enumerable.Empty<MedicineDto>();
    }
}
