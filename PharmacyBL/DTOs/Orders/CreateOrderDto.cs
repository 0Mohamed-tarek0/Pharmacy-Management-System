using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmacyBL.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public int SupplierId { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;

        public List<OrderItemInputDto> Items { get; set; } = new();
    }
}
