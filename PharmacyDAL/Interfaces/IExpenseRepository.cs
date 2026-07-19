using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetAllWithDetailsAsync();

        Task<Expense?> GetByIdWithDetailsAsync(int id);
    }
}
