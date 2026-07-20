using PharmacyBL.DTOs.Shifts;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;
using PharmacyDAL.UnitOfWork;

namespace PharmacyBL.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShiftService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ShiftDashboardDto> GetDashboardAsync(string applicationUserId, bool includeAllUsers = false)
        {
            var openShift = await _unitOfWork.Shifts.GetOpenByUserAsync(applicationUserId);
            var history = includeAllUsers
                ? await _unitOfWork.Shifts.GetAllClosedAsync()
                : await _unitOfWork.Shifts.GetClosedByUserAsync(applicationUserId);
            return new ShiftDashboardDto
            {
                OpenShift = openShift == null ? null : MapToDto(openShift),
                ShowsAllUsers = includeAllUsers,
                History = history.Select(MapToDto)
            };
        }

        public async Task<ShiftDto?> GetByIdAsync(int id, string applicationUserId, bool includeAllUsers = false)
        {
            var shift = await _unitOfWork.Shifts.GetByIdWithUserAsync(id);
            return shift == null || (!includeAllUsers && shift.ApplicationUserId != applicationUserId)
                ? null
                : MapToDto(shift);
        }

        public async Task<bool> OpenAsync(OpenShiftDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ApplicationUserId) || dto.OpeningCash < 0)
                return false;

            if (await _unitOfWork.Shifts.GetOpenByUserAsync(dto.ApplicationUserId) != null)
                return false;

            await _unitOfWork.Shifts.AddAsync(new Shift
            {
                ApplicationUserId = dto.ApplicationUserId,
                OpeningCash = dto.OpeningCash,
                OpenedAt = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ShiftDto?> CloseAsync(CloseShiftDto dto)
        {
            if (dto.ActualCash < 0 || string.IsNullOrWhiteSpace(dto.ApplicationUserId))
                return null;

            var shift = await _unitOfWork.Shifts.GetByIdWithUserAsync(dto.Id);
            if (shift == null || shift.ApplicationUserId != dto.ApplicationUserId || shift.ClosedAt != null)
                return null;

            var closedAt = DateTime.UtcNow;
            var sales = await _unitOfWork.Sales.GetTotalByUserAndDateRangeAsync(
                dto.ApplicationUserId, shift.OpenedAt, closedAt);
            var returns = await _unitOfWork.StockTransactions.GetSaleReturnValueByUserAndDateRangeAsync(
                dto.ApplicationUserId, shift.OpenedAt, closedAt);
            var expectedCash = shift.OpeningCash + sales - returns;

            shift.ClosedAt = closedAt;
            shift.SalesTotal = sales;
            shift.ReturnsTotal = returns;
            shift.ExpectedCash = expectedCash;
            shift.ActualCash = dto.ActualCash;
            shift.CashDifference = dto.ActualCash - expectedCash;

            _unitOfWork.Shifts.Update(shift);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(shift);
        }

        private static ShiftDto MapToDto(Shift shift) => new()
        {
            Id = shift.Id,
            CashierName = shift.ApplicationUser?.FullName ?? shift.ApplicationUser?.UserName ?? "Unknown",
            OpeningCash = shift.OpeningCash,
            OpenedAt = shift.OpenedAt,
            ClosedAt = shift.ClosedAt,
            SalesTotal = shift.SalesTotal,
            ReturnsTotal = shift.ReturnsTotal,
            ExpectedCash = shift.ExpectedCash,
            ActualCash = shift.ActualCash,
            CashDifference = shift.CashDifference
        };
    }
}
