using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        Task<Medicine> GetByBarcodeAsync(string barcode);

        /// <summary>Medicine with Category, Batches and Units eagerly loaded.</summary>
        Task<Medicine> GetMedicineWithDetailsAsync(int medicineId);

        /// <summary>Medicines whose total batch quantity is at/under their MinimumStock.</summary>
        Task<IEnumerable<Medicine>> GetLowStockMedicinesAsync();

        /// <summary>Medicines that have at least one batch expiring within the given days.</summary>
        Task<IEnumerable<Medicine>> GetExpiringSoonAsync(int daysThreshold);

        Task<IEnumerable<Medicine>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Medicine>> GetBySupplierAsync(int supplierId);

        /// <summary>All medicines with Category and Batches loaded (for stock totals).</summary>
        Task<IEnumerable<Medicine>> GetAllWithCategoryAsync();

        /// <summary>All medicines with Category, Batches, and Units loaded.</summary>
        Task<IEnumerable<Medicine>> GetAllWithDetailsAsync();
    }
}
