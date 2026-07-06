using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Repositories;

namespace PharmacyDAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext.ApplicationDbContext _context;

        private ICategoryRepository _categories;
        private IMedicineRepository _medicines;
        private ISupplierRepository _suppliers;
        private ISaleRepository _sales;
        private IOrderRepository _orders;

        public UnitOfWork(DbContext.ApplicationDbContext context)
        {
            _context = context;
        }

        // Repositories are created only when first accessed (lazy loading)
        public ICategoryRepository Categories =>
            _categories ??= new CategoryRepository(_context);

        public IMedicineRepository Medicines =>
            _medicines ??= new MedicineRepository(_context);

        public ISupplierRepository Suppliers =>
            _suppliers ??= new SupplierRepository(_context);

        public ISaleRepository Sales =>
            _sales ??= new SaleRepository(_context);

        public IOrderRepository Orders =>
            _orders ??= new OrderRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
