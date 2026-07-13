using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

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
        private IMedicineBatchRepository _medicineBatches;
        private IStockTransactionRepository _stockTransactions;
        private IGenericRepository<PharmacyDAL.Models.MedicineUnit> _medicineUnits;
        private IGenericRepository<PharmacyDAL.Models.MedicineSupplier> _medicineSuppliers;

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

        public IMedicineBatchRepository MedicineBatches =>
            _medicineBatches ??= new MedicineBatchRepository(_context);

        public IStockTransactionRepository StockTransactions =>
            _stockTransactions ??= new StockTransactionRepository(_context);

        public IGenericRepository<PharmacyDAL.Models.MedicineUnit> MedicineUnits =>
            _medicineUnits ??= new GenericRepository<PharmacyDAL.Models.MedicineUnit>(_context);

        public IGenericRepository<PharmacyDAL.Models.MedicineSupplier> MedicineSuppliers =>
            _medicineSuppliers ??= new GenericRepository<PharmacyDAL.Models.MedicineSupplier>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
