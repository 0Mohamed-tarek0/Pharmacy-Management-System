using PharmacyBL.DTOs.Financial;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Enums;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FinancialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task<DailyFinancialSummaryDto> GetDailySummaryAsync(DateTime? date = null)
        {
            // Normalise the target date to a UTC calendar day boundary.
            var targetDate = (date ?? DateTime.UtcNow).Date;
            var from = targetDate;
            var to   = targetDate.AddDays(1).AddTicks(-1); // 23:59:59.9999999

            // ── 1. Sales ──────────────────────────────────────────────────────
            var sales = await _unitOfWork.Sales.GetTotalByDateRangeAsync(from, to);

            // ── 2. Purchases (received orders only) ───────────────────────────
            var purchases = await _unitOfWork.Orders.GetTotalByDateRangeAsync(from, to);

            // ── 3. Expenses ───────────────────────────────────────────────────
            var expenses = await _unitOfWork.Expenses.GetTotalByDateRangeAsync(from, to);

            // ── 4. Sales Returns ──────────────────────────────────────────────
            // Each SaleReturn StockTransaction carries a positive Quantity (units
            // returned) and is linked to the MedicineBatch the units go back into.
            // Value = Quantity × batch.SellingPrice.
            var saleReturnTxs = await _unitOfWork.StockTransactions
                .GetByDateRangeWithBatchAsync(from, to, StockTransactionType.SaleReturn);

            var salesReturns = saleReturnTxs
                .Sum(t => t.MedicineBatch != null
                    ? t.Quantity * t.MedicineBatch.SellingPrice
                    : 0m);

            // ── 5. Purchase Returns ───────────────────────────────────────────
            // Each PurchaseReturn StockTransaction carries a negative Quantity
            // (units removed from stock and sent back). Value = |Quantity| × batch.PurchasePrice.
            var purchaseReturnTxs = await _unitOfWork.StockTransactions
                .GetByDateRangeWithBatchAsync(from, to, StockTransactionType.PurchaseReturn);

            var purchaseReturns = purchaseReturnTxs
                .Sum(t => t.MedicineBatch != null
                    ? Math.Abs(t.Quantity) * t.MedicineBatch.PurchasePrice
                    : 0m);

            return new DailyFinancialSummaryDto
            {
                Date            = targetDate,
                Sales           = sales,
                Purchases       = purchases,
                SalesReturns    = salesReturns,
                PurchaseReturns = purchaseReturns,
                Expenses        = expenses
            };
        }

        public async Task<OverallFinancialReportDto> GetOverallFinancialReportAsync()
        {
            var sales =
                await _unitOfWork.Sales.GetAllAsync();

            var orders =
                await _unitOfWork.Orders.GetAllAsync();

            var expenses =
                await _unitOfWork.Expenses.GetAllAsync();

            var saleReturnTransactions =
                await _unitOfWork.StockTransactions
                                 .GetByTypeWithBatchAsync(
                                   StockTransactionType.SaleReturn);

            var purchaseReturnTransactions =
               await _unitOfWork.StockTransactions
                                .GetByTypeWithBatchAsync(
                                StockTransactionType.PurchaseReturn);



            decimal totalSales =
                sales.Sum(x => x.TotalAmount);

            decimal totalPurchases =
                orders
                .Where(x => x.Status == OrderStatus.Received)
                .Sum(x => x.TotalAmount);

            decimal totalExpenses =
                expenses.Sum(x => x.Amount);



            decimal totalSalesReturns =
                saleReturnTransactions.Sum(x =>
                    x.MedicineBatch == null
                        ? 0
                        : x.Quantity * x.MedicineBatch.SellingPrice);

            decimal totalPurchaseReturns =
                purchaseReturnTransactions.Sum(x =>
                    x.MedicineBatch == null
                        ? 0
                        : Math.Abs(x.Quantity) * x.MedicineBatch.PurchasePrice);



            decimal netCashFlow =
                totalSales
                + totalPurchaseReturns
                - totalPurchases
                - totalSalesReturns
                - totalExpenses;



            return new OverallFinancialReportDto
            {
                TotalSales = totalSales,

                TotalPurchases = totalPurchases,

                TotalExpenses = totalExpenses,

                TotalSalesReturns = totalSalesReturns,

                TotalPurchaseReturns = totalPurchaseReturns,

                NetCashFlow = netCashFlow,

                TotalSalesInvoices = sales.Count(),

                TotalPurchaseOrders =
                    orders.Count(x => x.Status == OrderStatus.Received),

                TotalSalesReturnsCount =
                    saleReturnTransactions.Count(),

                TotalPurchaseReturnsCount =
                    purchaseReturnTransactions.Count()
            };
        }
    }
}
