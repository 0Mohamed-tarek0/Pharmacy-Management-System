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

        /// <summary>
        /// Cost basis (at purchase price) of every unit actually sold on this day,
        /// net of the cost basis restored by any customer returns on this day.
        /// This is the correct figure to compare against revenue for a profit
        /// calculation - unlike "Purchases", which reflects restocking cash flow
        /// and has no fixed relationship to what was sold on any given day.
        /// </summary>
        public decimal CostOfGoodsSold { get; set; }

        // ── Calculated figures ────────────────────────────────────────────────

        /// <summary>Sales − SalesReturns. The real revenue earned on this day.</summary>
        public decimal NetSales => Sales - SalesReturns;

        /// <summary>Purchases − PurchaseReturns. Cash spent restocking, not a cost of the day's sales.</summary>
        public decimal NetPurchases => Purchases - PurchaseReturns;

        /// <summary>
        /// NetSales − CostOfGoodsSold. The true margin earned on what was actually
        /// sold today, matching revenue against the cost of those exact units.
        /// </summary>
        public decimal GrossProfit => NetSales - CostOfGoodsSold;

        /// <summary>GrossProfit as a percentage of NetSales (0 when there were no net sales).</summary>
        public decimal GrossProfitMarginPercent => NetSales != 0 ? (GrossProfit / NetSales) * 100m : 0m;

        /// <summary>GrossProfit − Expenses. The day's bottom-line profit.</summary>
        public decimal NetProfit => GrossProfit - Expenses;

        /// <summary>NetProfit as a percentage of NetSales (0 when there were no net sales).</summary>
        public decimal NetProfitMarginPercent => NetSales != 0 ? (NetProfit / NetSales) * 100m : 0m;

        /// <summary>
        /// NetSales − NetPurchases − Expenses. Actual cash movement for the day
        /// (assumes cash sales/purchases) - a liquidity view, distinct from profit,
        /// since restocking spend and cost of goods sold rarely match on any given day.
        /// </summary>
        public decimal NetCashFlow => NetSales - NetPurchases - Expenses;
    }
}
