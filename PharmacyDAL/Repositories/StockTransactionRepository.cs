using Microsoft.EntityFrameworkCore;
using PharmacyDAL.Enums;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Models;

namespace PharmacyDAL.Repositories
{
    public class StockTransactionRepository : GenericRepository<StockTransaction>, IStockTransactionRepository
    {
        public StockTransactionRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StockTransaction>> GetByDateRangeWithBatchAsync(
            DateTime from,
            DateTime to,
            params StockTransactionType[] types)
        {
            var query = _dbSet
                .Include(t => t.MedicineBatch)
                .Where(t => t.TransactionDate >= from && t.TransactionDate <= to);

            if (types != null && types.Length > 0)
                query = query.Where(t => types.Contains(t.Type));

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<StockTransaction>> GetByTypeWithBatchAsync(
               params StockTransactionType[] types)
        {
            var query = _dbSet
                .Include(x => x.MedicineBatch)
                .AsQueryable();

            if (types != null && types.Any())
            {
                query = query.Where(x => types.Contains(x.Type));
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<StockTransaction>> GetReturnsWithDetailsAsync()
        {
            return await _dbSet
                .Include(t => t.Medicine)
                .Include(t => t.MedicineBatch)
                .Include(t => t.ApplicationUser)
                .Where(t => t.Type == StockTransactionType.PurchaseReturn
                         || t.Type == StockTransactionType.SaleReturn)
                .OrderByDescending(t => t.TransactionDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<decimal> GetSaleReturnValueByUserAndDateRangeAsync(string applicationUserId, DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(t => t.ApplicationUserId == applicationUserId
                    && t.TransactionDate >= from && t.TransactionDate <= to
                    && t.Type == StockTransactionType.SaleReturn)
                .SumAsync(t => (decimal?)(t.Quantity * (t.MedicineBatch == null ? 0m : t.MedicineBatch.SellingPrice))) ?? 0m;
        }
    }
}
