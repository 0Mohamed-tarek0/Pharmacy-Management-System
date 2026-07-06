using PharmacyBL.DTOs.Categories;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });

            return result;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return null;

            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return dto;

        }

        public async Task<bool> CreateAsync(CreateCategoryDto dto)
        {
            bool exists = await _unitOfWork.Categories
                          .ExistsAsync(c => c.Name == dto.Name);

            if (exists)
                return false;

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _unitOfWork.Categories.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);

            if (category == null)
                return false;

            bool exists = await _unitOfWork.Categories.ExistsAsync(c =>
                c.Name == dto.Name && c.Id != dto.Id);

            if (exists)
                return false;

            category.Name = dto.Name;
            category.Description = dto.Description;

            _unitOfWork.Categories.Update(category);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories
                                            .GetCategoryWithMedicinesAsync(id);

            if (category == null)
                return false;

            if (category.Medicines.Any())
                return false;

            _unitOfWork.Categories.Remove(category);

            await _unitOfWork.SaveChangesAsync();

            return true;

        }
    }
}