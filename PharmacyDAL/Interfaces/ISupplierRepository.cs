using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<Supplier> GetSupplierWithMedicinesAsync(int supplierId);

        Task<Supplier> GetSupplierWithOrdersAsync(int supplierId);
    }
}
