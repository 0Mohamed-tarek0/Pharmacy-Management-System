using PharmacyBL.DTOs.Shifts;

namespace PharmacyBL.Interfaces.Services
{
    public interface IShiftService
    {
        Task<ShiftDashboardDto> GetDashboardAsync(string applicationUserId, bool includeAllUsers = false);
        Task<ShiftDto?> GetByIdAsync(int id, string applicationUserId, bool includeAllUsers = false);
        Task<bool> OpenAsync(OpenShiftDto dto);
        Task<ShiftDto?> CloseAsync(CloseShiftDto dto);
    }
}
