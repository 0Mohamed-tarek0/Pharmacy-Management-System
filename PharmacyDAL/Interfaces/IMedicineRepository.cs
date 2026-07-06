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

        Task<Medicine> GetMedicineWithDetailsAsync(int medicineId);

        Task<IEnumerable<Medicine>> GetLowStockMedicinesAsync();

        Task<IEnumerable<Medicine>> GetExpiringSoonAsync(int daysThreshold);

        Task<IEnumerable<Medicine>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Medicine>> GetBySupplierAsync(int supplierId);
    }
}
