using PharmacyBL.DTOs.Expenses;

namespace PharmacyBL.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDto>> GetAllAsync();

        Task<ExpenseDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateExpenseDto dto);

        Task<bool> UpdateAsync(UpdateExpenseDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
