namespace PharmacyBL.DTOs.Shifts
{
    public class ShiftDto
    {
        public int Id { get; set; }
        public string CashierName { get; set; } = string.Empty;
        public decimal OpeningCash { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public decimal? SalesTotal { get; set; }
        public decimal? ReturnsTotal { get; set; }
        public decimal? ExpectedCash { get; set; }
        public decimal? ActualCash { get; set; }
        public decimal? CashDifference { get; set; }
        public bool IsOpen => ClosedAt == null;
    }
}
