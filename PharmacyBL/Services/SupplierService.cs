using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyBL.DTOs.Suppliers;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
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

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);

            if (supplier == null)
                return null;

            return new SupplierDto
            {
                Id = supplier.Id,
                CompanyName = supplier.CompanyName,
                Address = supplier.Address,
                Phone = supplier.Phone,
                Email = supplier.Email
            };
        }
        public async Task<bool> CreateAsync(CreateSupplierDto dto)
        {
            bool exists = await _unitOfWork.Suppliers.ExistsAsync(s => s.CompanyName.ToLower() == dto.CompanyName.ToLower());

            if (exists)
                return false;

            var supplier = new Supplier
            {
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email
            };

            await _unitOfWork.Suppliers.AddAsync(supplier);

            await _unitOfWork.SaveChangesAsync();

            return true;

        }

        public async Task<bool> UpdateAsync(UpdateSupplierDto dto)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(dto.Id);

            if (supplier == null)
                return false;

            bool exists = await _unitOfWork.Suppliers.ExistsAsync(s => s.CompanyName.ToLower() == dto.CompanyName.ToLower() && s.Id != dto.Id);

            if (exists)
                return false;

            supplier.CompanyName = dto.CompanyName;
            supplier.Address = dto.Address;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;

            _unitOfWork.Suppliers.Update(supplier);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _unitOfWork.Suppliers.GetSupplierWithRelationsAsync(id);

            if (supplier == null)
                return false;

            if(supplier.Orders.Any())
                return false;

            if(supplier.MedicineSuppliers.Any())
                return false;

            _unitOfWork.Suppliers.Remove(supplier);

            await _unitOfWork.SaveChangesAsync();

            return true;

        }

    }
}
