using System;
using System.ComponentModel.DataAnnotations;

namespace PharmacyBL.DTOs.Orders
{
    /// <summary>One row the pharmacist added to the temporary Order Entry table.</summary>
    public class OrderItemInputDto
    {
        [Required]
        public int MedicineId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>Unit the quantity was entered in, e.g. "Box" or "Strip". Empty = base unit.</summary>
        public string UnitName { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal PurchasePrice { get; set; }

        /// <summary>Discount as a percentage (0-100), applied to Quantity x PurchasePrice.</summary>
        [Range(0, 100)]
        public decimal Discount { get; set; }

        [Range(0, 100000)]
        public decimal SellingPrice { get; set; }

        [Required]
        public string BatchNumber { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
