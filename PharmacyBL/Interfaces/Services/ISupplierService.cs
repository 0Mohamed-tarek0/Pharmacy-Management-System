using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyBL.DTOs.Suppliers;

namespace PharmacyBL.Interfaces.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();

        Task<SupplierDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateSupplierDto dto);

        Task<bool> UpdateAsync(UpdateSupplierDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
