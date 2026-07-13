using System;
using System.Collections.Generic;
using PharmacyDAL.Enums;

namespace PharmacyBL.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }

    
}
