using System;
using System.Collections.Generic;
using PharmacyDAL.Enums;

namespace PharmacyBL.DTOs.Orders
{
    /// <summary>Order header plus every line item, for the Order Details screen.</summary>
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string SupplierName { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemViewDto> Items { get; set; } = new();
    }
}
