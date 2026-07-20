using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyBL.Common;
using PharmacyBL.DTOs.Medicines;
using PharmacyBL.DTOs.Sales;

namespace PharmacyBL.Interfaces.Services
{
    public interface ISaleService
    {
        /// <summary>
        /// Creates a Sale and deducts stock FEFO (First-Expire-First-Out):
        /// for every line, the requested quantity (converted to the base
        /// unit) is taken from the batch with the nearest expiry date first,
        /// then the next, and so on until the line is fully covered.
        /// </summary>
        Task<int> CreateAsync(CreateSaleDto dto);

        Task<IEnumerable<SaleDto>> GetAllAsync();

        Task<SaleDetailsDto?> GetDetailsAsync(int id);

        Task<IEnumerable<MedicineDto>> GetMedicinesAsync();

        Task<IEnumerable<MedicineUnitDto>> GetMedicineUnitsAsync(int medicineId);

        Task<decimal> GetTotalByUserAndDateRangeAsync(string applicationUserId, DateTime from, DateTime to);

        /// <summary>
        /// Returns some (or all) of a Sale line back from the customer: restocks the
        /// exact batch(es) it was originally sold from (traced via the Sale's
        /// StockTransaction history) and logs SaleReturn StockTransaction(s).
        /// </summary>
        Task<ServiceResult> ReturnItemAsync(ReturnSaleItemDto dto);
    }
}
