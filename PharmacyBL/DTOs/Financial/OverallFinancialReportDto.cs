using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Financial
{
    public class OverallFinancialReportDto
    {
        // Money
        public decimal TotalSales { get; set; }

        public decimal TotalPurchases { get; set; }

        public decimal TotalExpenses { get; set; }

        public decimal TotalSalesReturns { get; set; }

        public decimal TotalPurchaseReturns { get; set; }

        /// <summary>
        /// Cost basis (at purchase price) of every unit ever sold, net of the
        /// cost basis restored by customer returns. Correct counterpart to
        /// TotalSales for a real profit calculation (unlike TotalPurchases,
        /// which includes stock bought but not yet sold).
        /// </summary>
        public decimal TotalCostOfGoodsSold { get; set; }

        /// <summary>TotalSales − TotalSalesReturns.</summary>
        public decimal NetSales => TotalSales - TotalSalesReturns;

        /// <summary>TotalPurchases − TotalPurchaseReturns. Cash spent on restocking overall.</summary>
        public decimal NetPurchases => TotalPurchases - TotalPurchaseReturns;

        /// <summary>NetSales − TotalCostOfGoodsSold. The real all-time gross margin.</summary>
        public decimal GrossProfit => NetSales - TotalCostOfGoodsSold;

        /// <summary>GrossProfit as a percentage of NetSales (0 when there were no net sales).</summary>
        public decimal GrossProfitMarginPercent => NetSales != 0 ? (GrossProfit / NetSales) * 100m : 0m;

        /// <summary>GrossProfit − TotalExpenses. The true bottom-line profit.</summary>
        public decimal NetProfit => GrossProfit - TotalExpenses;

        /// <summary>NetProfit as a percentage of NetSales (0 when there were no net sales).</summary>
        public decimal NetProfitMarginPercent => NetSales != 0 ? (NetProfit / NetSales) * 100m : 0m;

        /// <summary>
        /// NetSales − NetPurchases − TotalExpenses. Cash movement view (assumes
        /// cash sales/purchases) - kept distinct from profit since restocking
        /// spend is not the same as the cost of goods actually sold.
        /// </summary>
        public decimal NetCashFlow => NetSales - NetPurchases - TotalExpenses;

        // Statistics
        public int TotalSalesInvoices { get; set; }

        public int TotalPurchaseOrders { get; set; }

        public int TotalSalesReturnsCount { get; set; }

        public int TotalPurchaseReturnsCount { get; set; }


    }
}
