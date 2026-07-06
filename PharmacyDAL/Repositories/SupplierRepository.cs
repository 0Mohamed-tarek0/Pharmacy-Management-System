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
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Supplier> GetSupplierWithMedicinesAsync(int supplierId)
        {
            return await _dbSet
                .Include(s => s.MedicineSuppliers)
                .ThenInclude(ms => ms.Medicine)
                .SingleOrDefaultAsync(s => s.Id == supplierId);
        }

        public async Task<Supplier> GetSupplierWithOrdersAsync(int supplierId)
        {
            return await _dbSet
                .Include(s => s.Orders)
                .ThenInclude(o => o.OrderItems)
                .SingleOrDefaultAsync(s => s.Id == supplierId);
        }
    }
}
