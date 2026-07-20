using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Sales
{
    public class ReturnSaleItemDto
    {
        /// <summary>Used only to redirect back to the correct Sale Details page.</summary>
        public int SaleId { get; set; }

        [Required]
        public int SaleItemId { get; set; }

        /// <summary>Quantity to return, in base units (same unit space as SaleItem.Quantity).</summary>
        [Range(1, int.MaxValue, ErrorMessage = "Return quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [MaxLength(300)]
        public string? Reason { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
