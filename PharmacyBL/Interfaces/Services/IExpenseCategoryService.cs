using PharmacyBL.DTOs.ExpenseCategories;

namespace PharmacyBL.Interfaces.Services
{
    public interface IExpenseCategoryService
    {
        Task<IEnumerable<ExpenseCategoryDto>> GetAllAsync();

        Task<ExpenseCategoryDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateExpenseCategoryDto dto);

        Task<bool> UpdateAsync(UpdateExpenseCategoryDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
