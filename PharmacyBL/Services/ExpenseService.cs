using PharmacyBL.DTOs.Expenses;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
        {
            var expenses = await _unitOfWork.Expenses.GetAllWithDetailsAsync();

            return expenses.Select(MapToDto);
        }

        public async Task<ExpenseDto?> GetByIdAsync(int id)
        {
            var expense = await _unitOfWork.Expenses.GetByIdWithDetailsAsync(id);

            if (expense == null)
                return null;

            return MapToDto(expense);
        }

        public async Task<bool> CreateAsync(CreateExpenseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ApplicationUserId))
                return false;

            bool categoryExists = await _unitOfWork.ExpenseCategories
                .ExistsAsync(c => c.Id == dto.ExpenseCategoryId);

            if (!categoryExists)
                return false;

            var expense = new Expense
            {
                Title = dto.Title.Trim(),
                Amount = dto.Amount,
                ExpenseCategoryId = dto.ExpenseCategoryId,
                ExpenseDate = dto.ExpenseDate.Date,
                Notes = dto.Notes?.Trim(),
                ApplicationUserId = dto.ApplicationUserId
            };

            await _unitOfWork.Expenses.AddAsync(expense);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateExpenseDto dto)
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(dto.Id);

            if (expense == null)
                return false;

            bool categoryExists = await _unitOfWork.ExpenseCategories
                .ExistsAsync(c => c.Id == dto.ExpenseCategoryId);

            if (!categoryExists)
                return false;

            expense.Title = dto.Title.Trim();
            expense.Amount = dto.Amount;
            expense.ExpenseCategoryId = dto.ExpenseCategoryId;
            expense.ExpenseDate = dto.ExpenseDate.Date;
            expense.Notes = dto.Notes?.Trim();

            _unitOfWork.Expenses.Update(expense);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(id);

            if (expense == null)
                return false;

            _unitOfWork.Expenses.Remove(expense);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static ExpenseDto MapToDto(Expense expense)
        {
            return new ExpenseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount = expense.Amount,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                ExpenseCategoryName = expense.ExpenseCategory?.Name ?? string.Empty,
                ExpenseDate = expense.ExpenseDate,
                Notes = expense.Notes,
                ApplicationUserId = expense.ApplicationUserId,
                RecordedBy = expense.ApplicationUser?.FullName
                    ?? expense.ApplicationUser?.UserName
                    ?? "Unknown"
            };
        }
    }
}
