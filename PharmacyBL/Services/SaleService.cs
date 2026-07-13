using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBL.DTOs.Sales;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Enums;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(CreateSaleDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                throw new Exception("A sale must have at least one item.");

            var sale = new Sale
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                InvoiceDate = DateTime.UtcNow,
                ApplicationUserId = dto.ApplicationUserId,
                Status = "Completed"
            };

            decimal saleTotal = 0;

            foreach (var itemDto in dto.Items)
            {
                var medicine = await _unitOfWork.Medicines.GetMedicineWithDetailsAsync(itemDto.MedicineId);
                if (medicine == null)
                    throw new Exception($"Medicine {itemDto.MedicineId} not found.");

                int conversionFactor = ResolveConversionFactor(medicine, itemDto.UnitName);
                int remainingBaseQuantity = itemDto.Quantity * conversionFactor;

                var fefoBatches = await _unitOfWork.MedicineBatches
                    .GetBatchesForMedicineFefoAsync(medicine.Id);

                int totalAvailable = fefoBatches.Sum(b => b.Quantity);
                if (totalAvailable < remainingBaseQuantity)
                    throw new Exception(
                        $"Not enough stock for {medicine.Name}. Requested {remainingBaseQuantity}, available {totalAvailable}.");

                decimal lineRevenue = 0;

                // Deduct from the batch with the nearest expiry date first (FEFO),
                // splitting across as many batches as needed.
                foreach (var batch in fefoBatches)
                {
                    if (remainingBaseQuantity <= 0)
                        break;

                    int takeFromBatch = Math.Min(batch.Quantity, remainingBaseQuantity);
                    if (takeFromBatch <= 0)
                        continue;

                    batch.Quantity -= takeFromBatch;
                    _unitOfWork.MedicineBatches.Update(batch);

                    await _unitOfWork.StockTransactions.AddAsync(new StockTransaction
                    {
                        MedicineId = medicine.Id,
                        MedicineBatchId = batch.Id,
                        Type = StockTransactionType.Sale,
                        Quantity = -takeFromBatch,
                        ReferenceType = "Sale",
                        Notes = $"Sold via {sale.InvoiceNumber}"
                    });

                    lineRevenue += takeFromBatch * batch.SellingPrice;
                    remainingBaseQuantity -= takeFromBatch;
                }

                var lineTotal = lineRevenue - itemDto.Discount;
                saleTotal += lineTotal;

                // Average unit price actually charged, for display purposes.
                var effectiveUnitPrice = itemDto.Quantity > 0
                    ? lineRevenue / (itemDto.Quantity * conversionFactor)
                    : 0;

                sale.SaleItems.Add(new SaleItem
                {
                    MedicineId = medicine.Id,
                    Quantity = itemDto.Quantity * conversionFactor,
                    UnitPrice = effectiveUnitPrice,
                    Discount = itemDto.Discount,
                    Total = lineTotal
                });
            }

            sale.TotalAmount = saleTotal;

            await _unitOfWork.Sales.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync();

            // Backfill the ReferenceId on the StockTransactions created above,
            // now that the Sale has an Id.
            var referenceIdUpdates = await _unitOfWork.StockTransactions
                .FindAsync(t => t.ReferenceType == "Sale" && t.ReferenceId == null
                    && t.Notes == $"Sold via {sale.InvoiceNumber}");

            foreach (var t in referenceIdUpdates)
            {
                t.ReferenceId = sale.Id;
                _unitOfWork.StockTransactions.Update(t);
            }

            await _unitOfWork.SaveChangesAsync();

            return sale.Id;
        }

        private static int ResolveConversionFactor(Medicine medicine, string unitName)
        {
            if (string.IsNullOrWhiteSpace(unitName))
                return 1;

            var unit = medicine.Units.FirstOrDefault(u =>
                string.Equals(u.UnitName, unitName, StringComparison.OrdinalIgnoreCase));

            return unit?.ConversionFactor ?? 1;
        }
    }
}
