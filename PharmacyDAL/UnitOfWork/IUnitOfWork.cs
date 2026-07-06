using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Interfaces;

namespace PharmacyDAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IMedicineRepository Medicines { get; }
        ISupplierRepository Suppliers { get; }
        ISaleRepository Sales { get; }
        IOrderRepository Orders { get; }

        Task<int> SaveChangesAsync();
    }
}
