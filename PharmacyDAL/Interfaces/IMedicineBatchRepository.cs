using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IMedicineBatchRepository : IGenericRepository<MedicineBatch>
    {
        /// <summary>Batch for a medicine matching a given batch number, if any.</summary>
        Task<MedicineBatch?> GetByBatchNumberAsync(int medicineId, string batchNumber);

        /// <summary>All batches for a medicine, ordered FEFO (earliest expiry first).</summary>
        Task<List<MedicineBatch>> GetBatchesForMedicineFefoAsync(int medicineId);

        Task<IEnumerable<MedicineBatch>> GetExpiringSoonAsync(int daysThreshold);
    }
}
