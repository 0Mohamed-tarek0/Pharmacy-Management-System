namespace PharmacyBL.DTOs.Financial
{
    /// <summary>
    /// Financial summary for a single calendar day.
    /// </summary>
    public class DailyFinancialSummaryDto
    {
        /// <summary>The date this summary covers.</summary>
        public DateTime Date { get; set; }

        // ── Raw figures ──────────────────────────────────────────────────────

        /// <summary>Total of all completed Sales invoices on this day.</summary>
        public decimal Sales { get; set; }

        /// <summary>Total of all received Purchase Orders on this day.</summary>
        public decimal Purchases { get; set; }

        /// <summary>
        /// Total value of goods returned by customers on this day
        /// (derived from SaleReturn StockTransactions × batch selling price).
        /// </summary>
        public decimal SalesReturns { get; set; }

        /// <summary>
        /// Total value of goods returned to suppliers on this day
        /// (derived from PurchaseReturn StockTransactions × batch purchase price).
        /// </summary>
        public decimal PurchaseReturns { get; set; }

        /// <summary>Total expenses recorded on this day.</summary>
        public decimal Expenses { get; set; }

        // ── Calculated figures ────────────────────────────────────────────────

        /// <summary>Sales − SalesReturns</summary>
        public decimal NetSales => Sales - SalesReturns;

        /// <summary>Purchases − PurchaseReturns</summary>
        public decimal NetPurchases => Purchases - PurchaseReturns;

        /// <summary>NetSales − NetPurchases</summary>
        public decimal GrossProfit => NetSales - NetPurchases;

        /// <summary>GrossProfit − Expenses</summary>
        public decimal NetProfit => GrossProfit - Expenses;
    }
}
