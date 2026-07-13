using System;

namespace PharmacyBL.DTOs.Medicines
{
    /// <summary>One batch row for the "all batches by nearest expiry" report.</summary>
    public class BatchExpiryRowDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }

        public bool IsExpired => ExpiryDate.Date < DateTime.Today;
    }
}
