using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace PharmacyDAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IMedicineRepository Medicines { get; }
        ISupplierRepository Suppliers { get; }
        ISaleRepository Sales { get; }
        IOrderRepository Orders { get; }
        IMedicineBatchRepository MedicineBatches { get; }
        IStockTransactionRepository StockTransactions { get; }
        IGenericRepository<PharmacyDAL.Models.MedicineUnit> MedicineUnits { get; }
        IGenericRepository<PharmacyDAL.Models.MedicineSupplier> MedicineSuppliers { get; }
        IGenericRepository<PharmacyDAL.Models.OrderItem> OrderItems { get; }
        IGenericRepository<PharmacyDAL.Models.SaleItem> SaleItems { get; }
        Task<int> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
