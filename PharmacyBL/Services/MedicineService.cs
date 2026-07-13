using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyBL.Common;
using PharmacyBL.DTOs.Categories;
using PharmacyBL.DTOs.Medicines;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MedicineDto>> GetAllAsync()
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
                    .OrderBy(b => b.ExpiryDate)
                    .Select(b => (decimal?)b.SellingPrice)
                    .FirstOrDefault()
            });
        }

        public async Task<UpdateMedicineDto?> GetByIdAsync(int id)
        {
            var medicine = await _unitOfWork.Medicines.GetByIdAsync(id);

            if (medicine == null)
                return null;

            return new UpdateMedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Type = medicine.Type,
                Description = medicine.Description,
                MinimumStock = medicine.MinimumStock,
                Barcode = medicine.Barcode,
                ImagePath = medicine.ImagePath,
                CategoryId = medicine.CategoryId
            };
        }

        public async Task<MedicineDetailsDto?> GetDetailsAsync(int id)
        {
            var medicine = await _unitOfWork.Medicines.GetMedicineWithDetailsAsync(id);

            if (medicine == null)
                return null;

            return new MedicineDetailsDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Type = medicine.Type,
                Description = medicine.Description,
                Barcode = medicine.Barcode,
                CategoryName = medicine.Category.Name,
                ImagePath = medicine.ImagePath,
                MinimumStock = medicine.MinimumStock,
                TotalQuantity = medicine.Batches.Sum(b => b.Quantity),
                Batches = medicine.Batches
                    .OrderBy(b => b.ExpiryDate)
                    .Select(b => new MedicineBatchDto
                    {
                        Id = b.Id,
                        BatchNumber = b.BatchNumber,
                        ExpiryDate = b.ExpiryDate,
                        PurchasePrice = b.PurchasePrice,
                        SellingPrice = b.SellingPrice,
                        Quantity = b.Quantity
                    }).ToList(),
                Units = medicine.Units
                    .OrderByDescending(u => u.ConversionFactor)
                    .Select(u => new MedicineUnitDto
                    {
                        Id = u.Id,
                        UnitName = u.UnitName,
                        ConversionFactor = u.ConversionFactor,
                        IsBaseUnit = u.IsBaseUnit
                    }).ToList()
            };
        }

        public async Task CreateAsync(CreateMedicineDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                bool exists = await _unitOfWork.Medicines
                    .ExistsAsync(m => m.Barcode == dto.Barcode);

                if (exists)
                    throw new Exception("Barcode already exists.");
            }

            var medicine = new Medicine
            {
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                MinimumStock = dto.MinimumStock,
                Barcode = dto.Barcode,
                ImagePath = dto.ImagePath,
                CategoryId = dto.CategoryId
            };

            // Every medicine gets its base (smallest sellable) unit registered
            // with a conversion factor of 1, so purchasing/selling logic always
            // has a unit to convert against.
            medicine.Units.Add(new MedicineUnit
            {
                UnitName = string.IsNullOrWhiteSpace(dto.BaseUnitName) ? "Unit" : dto.BaseUnitName,
                ConversionFactor = 1,
                IsBaseUnit = true
            });

            await _unitOfWork.Medicines.AddAsync(medicine);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateMedicineDto dto)
        {
            var medicine = await _unitOfWork.Medicines.GetByIdAsync(dto.Id);

            if (medicine == null)
                throw new Exception("Medicine not found.");

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                bool exists = await _unitOfWork.Medicines.ExistsAsync(m =>
                    m.Barcode == dto.Barcode &&
                    m.Id != dto.Id);

                if (exists)
                    throw new Exception("Barcode already exists.");
            }

            medicine.Name = dto.Name;
            medicine.Type = dto.Type;
            medicine.Description = dto.Description;
            medicine.MinimumStock = dto.MinimumStock;
            medicine.Barcode = dto.Barcode;
            medicine.ImagePath = dto.ImagePath;
            medicine.CategoryId = dto.CategoryId;

            _unitOfWork.Medicines.Update(medicine);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var medicine = await _unitOfWork.Medicines.GetByIdAsync(id);

            if (medicine == null)
                throw new Exception("Medicine not found.");

            _unitOfWork.Medicines.Remove(medicine);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }
    }
}
