using System;

namespace PharmacyBL.DTOs.Medicines
{
    public class MedicineBatchDto
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsExpired => ExpiryDate.Date < DateTime.Today;
    }
}
