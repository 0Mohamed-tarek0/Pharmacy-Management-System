namespace PharmacyBL.DTOs.Shifts
{
    public class ShiftDashboardDto
    {
        public ShiftDto? OpenShift { get; set; }
        public bool ShowsAllUsers { get; set; }
        public IEnumerable<ShiftDto> History { get; set; } = Enumerable.Empty<ShiftDto>();
    }
}
