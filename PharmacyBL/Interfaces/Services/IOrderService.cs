using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyBL.Common;
using PharmacyBL.DTOs.Categories;
using PharmacyBL.DTOs.Medicines;
using PharmacyBL.DTOs.Orders;
using PharmacyBL.DTOs.Suppliers;

namespace PharmacyBL.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();

        /// <summary>Order header plus every line item, for the Order Details screen.</summary>
        Task<OrderDetailsDto?> GetDetailsAsync(int id);

        Task<IEnumerable<SupplierDto>> GetSuppliersAsync();

        Task<IEnumerable<MedicineDto>> GetMedicinesAsync();

        /// <summary>
        /// Creates the purchase Order and, for every line, receives stock:
        /// converts the entered quantity to the medicine's base unit and
        /// creates/updates the matching MedicineBatch, logging a Purchase
        /// StockTransaction for each line.
        /// </summary>
        Task<int> CreateAsync(CreateOrderDto dto);

        Task<IEnumerable<MedicineUnitDto>> GetMedicineUnitsAsync(int medicineId);

        /// <summary>
        /// Returns some (or all) of an Order line back to the supplier: removes the
        /// quantity from the linked MedicineBatch and logs a PurchaseReturn StockTransaction.
        /// </summary>
        Task<ServiceResult> ReturnItemAsync(ReturnOrderItemDto dto);
    }
}
