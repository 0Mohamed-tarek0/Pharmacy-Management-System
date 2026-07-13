using System.ComponentModel.DataAnnotations;

namespace PharmacyBL.DTOs.Sales
{
    public class SaleItemInputDto
    {
        [Required]
        public int MedicineId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>Unit the quantity was entered in, e.g. "Box" or "Strip". Empty = base unit.</summary>
        public string UnitName { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal Discount { get; set; }
    }
}
