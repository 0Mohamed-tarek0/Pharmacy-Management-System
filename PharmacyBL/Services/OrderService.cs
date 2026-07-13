using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyBL.DTOs.Categories;
using PharmacyBL.DTOs.Medicines;
using PharmacyBL.DTOs.Orders;
using PharmacyBL.DTOs.Suppliers;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Enums;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllWithDetailsAsync();

            return orders
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    SupplierName = o.Supplier?.CompanyName ?? string.Empty,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                });
        }

        public async Task<OrderDetailsDto?> GetDetailsAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(id);

            if (order == null)
                return null;

            return new OrderDetailsDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                SupplierName = order.Supplier?.CompanyName ?? string.Empty,
                CreatedByUserName = order.ApplicationUser?.FullName ?? string.Empty,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemViewDto
                {
                    MedicineName = oi.Medicine?.Name ?? string.Empty,
                    UnitName = oi.UnitName,
                    Quantity = oi.Quantity,
                    PurchasePrice = oi.PurchasePrice,
                    SellingPrice = oi.SellingPrice,
                    Discount = oi.Discount,
                    BatchNumber = oi.BatchNumber,
                    ExpiryDate = oi.ExpiryDate,
                    Total = oi.Total
                }).ToList()
            };
        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliersAsync()
        {
            var suppliers = await _unitOfWork.Suppliers.GetAllAsync();

            return suppliers.Select(s => new SupplierDto
            {
                Id = s.Id,
                CompanyName = s.CompanyName,
                Address = s.Address,
                Phone = s.Phone,
                Email = s.Email
            });
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicinesAsync()
        {
            var medicines = await _unitOfWork.Medicines.GetAllWithCategoryAsync();

            return medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Type = m.Type,
                Barcode = m.Barcode,
                CategoryName = m.Category.Name,
                ImagePath = m.ImagePath,
                MinimumStock = m.MinimumStock,

                TotalQuantity = m.Batches.Sum(b => b.Quantity),

                SellingPrice = m.Batches
                                .Where(b => b.Quantity > 0)
                                .OrderByDescending(b => b.CreatedDate)
                                .ThenByDescending(b => b.SellingPrice)
                                .Select(b => (decimal?)b.SellingPrice)
                                .FirstOrDefault(),

                Units = m.Units.Select(u => new MedicineUnitDto
                {
                    UnitName = u.UnitName,
                    ConversionFactor = u.ConversionFactor,
                    IsBaseUnit = u.IsBaseUnit
                }).ToList()


            });
        }

        public async Task<int> CreateAsync(CreateOrderDto dto)
        {
            using var dbTransaction =
            await _unitOfWork.BeginTransactionAsync();

            try
            {

                if (dto.Items == null || dto.Items.Count == 0)
                    throw new Exception("An order must have at least one item.");

                var order = new Order
                {
                    OrderNumber = GenerateOrderNumber(),
                    SupplierId = dto.SupplierId,
                    ApplicationUserId = dto.ApplicationUserId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Received
                };

                decimal orderTotal = 0;

                foreach (var itemDto in dto.Items)
                {
                    var medicine = await _unitOfWork.Medicines
                                                    .GetMedicineWithDetailsAsync(itemDto.MedicineId);

                    if (medicine == null)
                        throw new Exception("Medicine not found.");

                    if (!string.IsNullOrWhiteSpace(itemDto.UnitName))
                    {
                        bool unitExists = medicine.Units.Any(u =>
                            u.UnitName.Equals(itemDto.UnitName,
                            StringComparison.OrdinalIgnoreCase));

                        if (!unitExists)
                            throw new Exception($"Unit '{itemDto.UnitName}' does not exist for {medicine.Name}.");
                    }

                    if (itemDto.ExpiryDate <= DateTime.Today)
                        throw new Exception($"{medicine.Name} has an expired batch.");



                    // 1. Resolve the unit the pharmacist entered the quantity in,
                    //    and convert it to the medicine's base (smallest sellable) unit.
                    int conversionFactor = ResolveConversionFactor(medicine, itemDto.UnitName);

                    if (conversionFactor <= 0)
                        throw new Exception("Invalid unit conversion.");
                    int baseQuantity = itemDto.Quantity * conversionFactor;

                    // Discount is a percentage (0-100), applied to Quantity x Purchase Price.
                    if (itemDto.Discount < 0 || itemDto.Discount > 100)
                        throw new Exception("Discount must be a percentage between 0 and 100.");

                    var lineTotal = (itemDto.Quantity * itemDto.PurchasePrice) * (1 - (itemDto.Discount / 100m));
                    orderTotal += lineTotal;

                    var orderItem = new OrderItem
                    {
                        MedicineId = medicine.Id,
                        Quantity = itemDto.Quantity,
                        UnitName = string.IsNullOrWhiteSpace(itemDto.UnitName)
                            ? BaseUnitName(medicine)
                            : itemDto.UnitName,
                        PurchasePrice = itemDto.PurchasePrice,
                        Discount = itemDto.Discount,
                        Total = lineTotal,
                        BatchNumber = itemDto.BatchNumber,
                        ExpiryDate = itemDto.ExpiryDate,
                        SellingPrice = itemDto.SellingPrice
                    };

                    // 2. Receive stock: if this Medicine + BatchNumber already exists,
                    //    top up its quantity (and refresh price per system policy);
                    //    otherwise create a brand new batch. A new Medicine is never
                    //    created here - Medicine identity must already exist.
                    if (itemDto.PurchasePrice <= 0)
                        throw new Exception("Purchase price must be greater than zero.");

                    if (itemDto.SellingPrice <= 0)
                        throw new Exception("Selling price must be greater than zero.");

                    if (itemDto.SellingPrice < itemDto.PurchasePrice)
                        throw new Exception($"Selling price of {medicine.Name} cannot be less than purchase price.");

                    if (itemDto.Quantity <= 0)
                        throw new Exception("Quantity must be greater than zero.");

                    var batch = await _unitOfWork.MedicineBatches
                        .GetByBatchNumberAsync(medicine.Id, itemDto.BatchNumber);

                    if (batch == null)
                    {

                        batch = new MedicineBatch
                        {
                            MedicineId = medicine.Id,
                            SupplierId = dto.SupplierId,
                            BatchNumber = itemDto.BatchNumber,
                            ExpiryDate = itemDto.ExpiryDate,
                            PurchasePrice = itemDto.PurchasePrice,
                            SellingPrice = itemDto.SellingPrice,
                            Quantity = baseQuantity
                        };

                        await _unitOfWork.MedicineBatches.AddAsync(batch);
                    }
                    else
                    {
                        // Same batch received again: add quantity and refresh prices.
                        batch.Quantity += baseQuantity;
                        batch.PurchasePrice = itemDto.PurchasePrice;
                        batch.SellingPrice = itemDto.SellingPrice;
                        batch.SupplierId = dto.SupplierId;
                        _unitOfWork.MedicineBatches.Update(batch);
                    }




                    orderItem.MedicineBatch = batch;
                    order.OrderItems.Add(orderItem);
                }

                order.TotalAmount = orderTotal;

                if (order.OrderItems.Count == 0)
                    throw new Exception("Order has no valid items.");

                await _unitOfWork.Orders.AddAsync(order);

                // Save once so the Order/OrderItems/Batches all get Ids...
                await _unitOfWork.SaveChangesAsync();

                // ...then log one Purchase StockTransaction per line, referencing the Order.
                foreach (var orderItem in order.OrderItems)
                {
                    var medicine = await _unitOfWork.Medicines.GetMedicineWithDetailsAsync(orderItem.MedicineId);
                    int conversionFactor = ResolveConversionFactor(medicine, orderItem.UnitName);

                    var transaction = new StockTransaction
                    {
                        MedicineId = orderItem.MedicineId,
                        MedicineBatchId = orderItem.MedicineBatchId,
                        Type = StockTransactionType.Purchase,
                        Quantity = orderItem.Quantity * conversionFactor,
                        ReferenceType = "Order",
                        ReferenceId = order.Id,
                        Notes = $"Received via Order {order.OrderNumber}"
                    };

                    await _unitOfWork.StockTransactions.AddAsync(transaction);
                }

                await _unitOfWork.SaveChangesAsync();

                await dbTransaction.CommitAsync();

                return order.Id;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        private static int ResolveConversionFactor(Medicine medicine, string unitName)
        {
            if (string.IsNullOrWhiteSpace(unitName))
                return 1;

            var unit = medicine.Units.FirstOrDefault(u =>
                string.Equals(u.UnitName, unitName, StringComparison.OrdinalIgnoreCase));

            return unit?.ConversionFactor ?? 1;
        }

        private static string BaseUnitName(Medicine medicine)
        {
            return medicine.Units.FirstOrDefault(u => u.IsBaseUnit)?.UnitName ?? "Unit";
        }

        private static string GenerateOrderNumber()
        {
            return $"PO-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        public async Task<IEnumerable<MedicineUnitDto>> GetMedicineUnitsAsync(int medicineId)
        {
            var medicine = await _unitOfWork.Medicines
                                            .GetMedicineWithDetailsAsync(medicineId);

            if (medicine == null)
                return Enumerable.Empty<MedicineUnitDto>();

            return medicine.Units
                .OrderByDescending(u => u.IsBaseUnit)
                .Select(u => new MedicineUnitDto
                {
                    UnitName = u.UnitName,
                    ConversionFactor = u.ConversionFactor,
                    IsBaseUnit = u.IsBaseUnit
                });
        }
    }
}
