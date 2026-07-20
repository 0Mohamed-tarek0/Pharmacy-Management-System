using PharmacyDAL.Enums;
using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IStockTransactionRepository : IGenericRepository<StockTransaction>
    {
        /// <summary>
        /// Returns all StockTransactions of the given type(s) whose
        /// <see cref="StockTransaction.TransactionDate"/> falls within [from, to],
        /// with the related <see cref="MedicineBatch"/> eagerly loaded so that
        /// purchase/selling prices are available without further DB round-trips.
        /// </summary>
        Task<IEnumerable<StockTransaction>> GetByDateRangeWithBatchAsync(
            DateTime from,
            DateTime to,
            params StockTransactionType[] types);

        Task<IEnumerable<StockTransaction>> GetByTypeWithBatchAsync(
            params StockTransactionType[] types);

        Task<IEnumerable<StockTransaction>> GetReturnsWithDetailsAsync();
    }
}
