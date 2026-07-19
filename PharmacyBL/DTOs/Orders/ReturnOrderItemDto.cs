using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Orders
{
    public class ReturnOrderItemDto
    {
        /// <summary>Used only to redirect back to the correct Order Details page.</summary>
        public int OrderId { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        /// <summary>Quantity to return, in the same unit the order line was placed in.</summary>
        [Range(1, int.MaxValue, ErrorMessage = "Return quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [MaxLength(300)]
        public string? Reason { get; set; }
    }
}
