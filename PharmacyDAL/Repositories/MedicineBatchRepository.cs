using Microsoft.EntityFrameworkCore;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Models;

namespace PharmacyDAL.Repositories
{
    public class MedicineBatchRepository : GenericRepository<MedicineBatch>, IMedicineBatchRepository
    {
        public MedicineBatchRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<MedicineBatch?> GetByBatchNumberAsync(int medicineId, string batchNumber)
        {
            return await _dbSet
                .SingleOrDefaultAsync(b => b.MedicineId == medicineId && b.BatchNumber == batchNumber);
        }

        public async Task<List<MedicineBatch>> GetBatchesForMedicineFefoAsync(int medicineId)
        {
            return await _dbSet
                .Where(b => b.MedicineId == medicineId && b.Quantity > 0)
                .OrderBy(b => b.ExpiryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicineBatch>> GetExpiringSoonAsync(int daysThreshold)
        {
            var cutoffDate = DateTime.Today.AddDays(daysThreshold);
            return await _dbSet
                .Include(b => b.Medicine)
                .Where(b => b.ExpiryDate <= cutoffDate && b.Quantity > 0)
                .ToListAsync();
        }
    }
}
