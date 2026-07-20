using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBL.Common;
using PharmacyBL.DTOs.Sales;
using PharmacyBL.DTOs.Medicines;
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
            using var dbTransaction = await _unitOfWork.BeginTransactionAsync();
            try
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

                var stockTransactions = new List<StockTransaction>();

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

                        var transaction = new StockTransaction
                        {
                            MedicineId = medicine.Id,
                            MedicineBatchId = batch.Id,
                            Type = StockTransactionType.Sale,
                            Quantity = -takeFromBatch,
                            ReferenceType = "Sale",
                            Notes = $"Sold via {sale.InvoiceNumber}"
                        };

                        await _unitOfWork.StockTransactions.AddAsync(transaction);
                        stockTransactions.Add(transaction);

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
                foreach (var t in stockTransactions)
                {
                    t.ReferenceId = sale.Id;
                }

                await _unitOfWork.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return sale.Id;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<SaleDto>> GetAllAsync()
        {
            var sales = await _unitOfWork.Sales.GetAllWithDetailsAsync();

            return sales.Select(s => new SaleDto
            {
                Id = s.Id,
                InvoiceNumber = s.InvoiceNumber,
                InvoiceDate = s.InvoiceDate,
                CashierName = s.ApplicationUser?.FullName ?? "Unknown",
                TotalAmount = s.TotalAmount,
                Status = s.Status
            });
        }

        public async Task<SaleDetailsDto?> GetDetailsAsync(int id)
        {
            var sale = await _unitOfWork.Sales.GetSaleWithItemsAsync(id);

            if (sale == null)
                return null;

            return new SaleDetailsDto
            {
                Id = sale.Id,
                InvoiceNumber = sale.InvoiceNumber,
                InvoiceDate = sale.InvoiceDate,
                CashierName = sale.ApplicationUser?.FullName ?? "Unknown",
                TotalAmount = sale.TotalAmount,
                Status = sale.Status,
                Items = sale.SaleItems.Select(si => new SaleItemViewDto
                {
                    Id = si.Id,
                    MedicineName = si.Medicine?.Name ?? "Unknown",
                    Quantity = si.Quantity,
                    UnitPrice = si.UnitPrice,
                    Discount = si.Discount,
                    Total = si.Total,
                    ReturnedQuantity = si.ReturnedQuantity
                }).ToList()
            };
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicinesAsync()
        {
            var medicines = await _unitOfWork.Medicines.GetAllWithDetailsAsync();

            return medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Type = m.Type,
                Barcode = m.Barcode,
                CategoryName = m.Category?.Name ?? "Unknown",
                ImagePath = m.ImagePath,
                MinimumStock = m.MinimumStock,
                TotalQuantity = m.Batches.Sum(b => b.Quantity),
                SellingPrice = m.Batches
                                .Where(b => b.Quantity > 0)
                                .OrderBy(b => b.ExpiryDate)
                                .Select(b => (decimal?)b.SellingPrice)
                                .FirstOrDefault(),
                Units = m.Units.Select(u => new MedicineUnitDto
                {
                    Id = u.Id,
                    UnitName = u.UnitName,
                    ConversionFactor = u.ConversionFactor,
                    IsBaseUnit = u.IsBaseUnit
                }).ToList()
            });
        }

        public async Task<IEnumerable<MedicineUnitDto>> GetMedicineUnitsAsync(int medicineId)
        {
            var units = await _unitOfWork.MedicineUnits.FindAsync(u => u.MedicineId == medicineId);
            return units.Select(u => new MedicineUnitDto
            {
                Id = u.Id,
                UnitName = u.UnitName,
                ConversionFactor = u.ConversionFactor,
                IsBaseUnit = u.IsBaseUnit
            });
        }

        public Task<decimal> GetTotalByUserAndDateRangeAsync(string applicationUserId, DateTime from, DateTime to)
        {
            return _unitOfWork.Sales.GetTotalByUserAndDateRangeAsync(applicationUserId, from, to);
        }

        private static int ResolveConversionFactor(Medicine medicine, string unitName)
        {
            if (string.IsNullOrWhiteSpace(unitName))
                return 1;

            var unit = medicine.Units.FirstOrDefault(u =>
                string.Equals(u.UnitName, unitName, StringComparison.OrdinalIgnoreCase));

            return unit?.ConversionFactor ?? 1;
        }

        /// <summary>
        /// Returns some (or all) of a Sale line back from the customer. Since a sale can
        /// be fulfilled FEFO from several batches, the batch(es) to restock are traced back
        /// from this Sale's own "Sale" StockTransactions (in the order they were consumed),
        /// skipping whatever has already been returned against each one.
        /// </summary>
        public async Task<ServiceResult> ReturnItemAsync(ReturnSaleItemDto dto)
        {
            using var dbTransaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (dto.Quantity <= 0)
                    throw new Exception("Return quantity must be greater than zero.");

                var saleItem = await _unitOfWork.SaleItems.SingleOrDefaultAsync(si => si.Id == dto.SaleItemId);
                if (saleItem == null)
                    throw new Exception("Sale item not found.");

                int remainingReturnable = saleItem.Quantity - saleItem.ReturnedQuantity;
                if (dto.Quantity > remainingReturnable)
                    throw new Exception(
                        $"Cannot return {dto.Quantity} unit(s); only {remainingReturnable} left to return on this line.");

                // The batch(es) this line was originally sold from, in the order they were consumed.
                var saleTransactions = (await _unitOfWork.StockTransactions.FindAsync(t =>
                        t.ReferenceType == "Sale" &&
                        t.ReferenceId == saleItem.SaleId &&
                        t.MedicineId == saleItem.MedicineId &&
                        t.Type == StockTransactionType.Sale))
                    .OrderBy(t => t.Id)
                    .ToList();

                if (!saleTransactions.Any())
                    throw new Exception("No original stock movement was found for this sale line; cannot determine which batch to restock.");

                // How much has already been returned per batch, from previous partial returns.
                var alreadyReturnedByBatch = (await _unitOfWork.StockTransactions.FindAsync(t =>
                        t.ReferenceType == "Sale" &&
                        t.ReferenceId == saleItem.SaleId &&
                        t.MedicineId == saleItem.MedicineId &&
                        t.Type == StockTransactionType.SaleReturn))
                    .GroupBy(t => t.MedicineBatchId)
                    .ToDictionary(g => g.Key, g => g.Sum(t => t.Quantity));

                int remainingToReturn = dto.Quantity;

                foreach (var tx in saleTransactions)
                {
                    if (remainingToReturn <= 0)
                        break;

                    if (tx.MedicineBatchId == null)
                        continue;

                    int soldFromBatch = -tx.Quantity;
                    alreadyReturnedByBatch.TryGetValue(tx.MedicineBatchId, out int alreadyReturned);

                    int availableToReturnFromBatch = soldFromBatch - alreadyReturned;
                    if (availableToReturnFromBatch <= 0)
                        continue;

                    int takeBack = Math.Min(availableToReturnFromBatch, remainingToReturn);

                    var batch = await _unitOfWork.MedicineBatches.GetByIdAsync(tx.MedicineBatchId.Value);
                    if (batch == null)
                        continue;

                    batch.Quantity += takeBack;
                    _unitOfWork.MedicineBatches.Update(batch);

                    var returnTransaction = new StockTransaction
                    {
                        MedicineId = saleItem.MedicineId,
                        MedicineBatchId = batch.Id,
                        Type = StockTransactionType.SaleReturn,
                        Quantity = takeBack,
                        ReferenceType = "Sale",
                        ReferenceId = saleItem.SaleId,
                        ApplicationUserId = dto.ApplicationUserId,
                        Notes = string.IsNullOrWhiteSpace(dto.Reason)
                            ? $"Customer return from sale item #{saleItem.Id}"
                            : dto.Reason
                    };

                    await _unitOfWork.StockTransactions.AddAsync(returnTransaction);

                    // Keep the running total up to date in case this loop touches the same batch twice.
                    alreadyReturnedByBatch[tx.MedicineBatchId] = alreadyReturned + takeBack;

                    remainingToReturn -= takeBack;
                }

                if (remainingToReturn > 0)
                    throw new Exception("Could not match the full return quantity to the original sale batches. Please check the sale's stock history.");

                saleItem.ReturnedQuantity += dto.Quantity;
                _unitOfWork.SaleItems.Update(saleItem);

                await _unitOfWork.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return new ServiceResult { Success = true, Message = "Customer return recorded and stock updated." };
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                return new ServiceResult { Success = false, Message = ex.Message };
            }
        }
    }
}
