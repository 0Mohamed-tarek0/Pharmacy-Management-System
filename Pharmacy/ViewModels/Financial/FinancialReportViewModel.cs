using PharmacyBL.DTOs.Financial;

namespace Pharmacy.ViewModels.Financial
{
    public class FinancialReportViewModel
    {
        public string Period { get; set; } = "today";
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string PeriodLabel { get; set; } = string.Empty;
        public DailyFinancialSummaryDto Summary { get; set; } = null!;
    }
}
