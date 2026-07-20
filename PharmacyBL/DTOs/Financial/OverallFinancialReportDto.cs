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

        public decimal NetCashFlow { get; set; }

        // Statistics
        public int TotalSalesInvoices { get; set; }

        public int TotalPurchaseOrders { get; set; }

        public int TotalSalesReturnsCount { get; set; }

        public int TotalPurchaseReturnsCount { get; set; }

        
    }
}
