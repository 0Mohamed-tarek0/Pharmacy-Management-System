using PharmacyBL.DTOs.ExpenseCategories;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.ExpenseCategories.GetAllAsync();

            return categories
                .OrderBy(c => c.Name)
                .Select(c => new ExpenseCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                });
        }

        public async Task<ExpenseCategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.ExpenseCategories.GetByIdAsync(id);

            if (category == null)
                return null;

            return new ExpenseCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<bool> CreateAsync(CreateExpenseCategoryDto dto)
        {
            bool exists = await _unitOfWork.ExpenseCategories
                .ExistsAsync(c => c.Name == dto.Name);

            if (exists)
                return false;

            var category = new ExpenseCategory
            {
                Name = dto.Name
            };

            await _unitOfWork.ExpenseCategories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateExpenseCategoryDto dto)
        {
            var category = await _unitOfWork.ExpenseCategories.GetByIdAsync(dto.Id);

            if (category == null)
                return false;

            bool exists = await _unitOfWork.ExpenseCategories
                .ExistsAsync(c => c.Name == dto.Name && c.Id != dto.Id);

            if (exists)
                return false;

            category.Name = dto.Name;

            _unitOfWork.ExpenseCategories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.ExpenseCategories
                .GetCategoryWithExpensesAsync(id);

            if (category == null)
                return false;

            if (category.Expenses.Any())
                return false;

            _unitOfWork.ExpenseCategories.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
