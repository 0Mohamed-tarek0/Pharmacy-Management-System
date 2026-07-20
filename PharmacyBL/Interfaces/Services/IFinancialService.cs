using PharmacyBL.DTOs.Financial;

namespace PharmacyBL.Interfaces.Services
{
    public interface IFinancialService
    {
        /// <summary>
        /// Returns a financial summary for a single calendar day.
        /// Defaults to today (UTC) when <paramref name="date"/> is null.
        /// </summary>
        Task<DailyFinancialSummaryDto> GetDailySummaryAsync(DateTime? date = null);

        /// <summary>
        /// Returns the cumulative financial summary across ALL recorded data
        /// (no date filter). The <see cref="DailyFinancialSummaryDto.Date"/>
        /// property will be set to <see cref="DateTime.MinValue"/> to signal
        /// that this is an all-time figure.
        /// </summary>
        //Task<DailyFinancialSummaryDto> GetAllTimeSummaryAsync();

        Task<OverallFinancialReportDto> GetOverallFinancialReportAsync();
    }
}
