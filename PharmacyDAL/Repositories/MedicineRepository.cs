using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Models;

namespace PharmacyDAL.Repositories
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Medicine> GetByBarcodeAsync(string barcode)
        {
            return await _dbSet.SingleOrDefaultAsync(m => m.Barcode == barcode);
        }

        public async Task<Medicine> GetMedicineWithDetailsAsync(int medicineId)
        {
            return await _dbSet
                
                .Include(m => m.Category)
                .Include(m => m.Batches)
                .Include(m => m.Units)
                .Include(m => m.MedicineSuppliers)
                    .ThenInclude(ms => ms.Supplier)
                .SingleOrDefaultAsync(m => m.Id == medicineId);
        }

        public async Task<IEnumerable<Medicine>> GetLowStockMedicinesAsync()
        {
            return await _dbSet
                .Include(m => m.Batches)
                .Where(m => (m.Batches.Sum(b => (int?)b.Quantity) ?? 0) <= m.MinimumStock)
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetExpiringSoonAsync(int daysThreshold)
        {
            var cutoffDate = DateTime.Today.AddDays(daysThreshold);
            return await _dbSet
                .Include(m => m.Batches)
                .Where(m => m.Batches.Any(b => b.ExpiryDate <= cutoffDate && b.Quantity > 0))
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(m => m.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetBySupplierAsync(int supplierId)
        {
            return await _dbSet
                .Where(m => m.MedicineSuppliers.Any(ms => ms.SupplierId == supplierId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetAllWithCategoryAsync()
        {
            return await _dbSet
                .Include(m => m.Category)
                .Include(m => m.Batches)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
