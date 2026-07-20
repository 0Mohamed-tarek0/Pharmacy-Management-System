using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IShiftRepository : IGenericRepository<Shift>
    {
        Task<Shift?> GetOpenByUserAsync(string applicationUserId);
        Task<Shift?> GetByIdWithUserAsync(int id);
        Task<IEnumerable<Shift>> GetClosedByUserAsync(string applicationUserId);
        Task<IEnumerable<Shift>> GetAllClosedAsync();
    }
}
