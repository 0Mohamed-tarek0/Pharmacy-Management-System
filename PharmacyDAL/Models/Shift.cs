namespace PharmacyDAL.Models
{
    public class Shift
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public decimal OpeningCash { get; set; }
        public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        // These are recorded when the shift is closed, preserving its reconciliation.
        public decimal? SalesTotal { get; set; }
        public decimal? ReturnsTotal { get; set; }
        public decimal? ExpectedCash { get; set; }
        public decimal? ActualCash { get; set; }
        public decimal? CashDifference { get; set; }
    }
}
