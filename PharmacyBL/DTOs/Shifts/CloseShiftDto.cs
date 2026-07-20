namespace PharmacyBL.DTOs.Shifts
{
    public class CloseShiftDto
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public decimal ActualCash { get; set; }
    }
}
