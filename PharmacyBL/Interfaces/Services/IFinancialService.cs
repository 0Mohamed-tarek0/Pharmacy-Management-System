using PharmacyBL.DTOs.Financial;

namespace PharmacyBL.Interfaces.Services
{
    public interface IFinancialService
    {
        
        Task<DailyFinancialSummaryDto> GetDailySummaryAsync(DateTime? date = null);

        Task<DailyFinancialSummaryDto> GetSummaryForRangeAsync(DateTime from, DateTime to);

        

        Task<OverallFinancialReportDto> GetOverallFinancialReportAsync();
    }
}
