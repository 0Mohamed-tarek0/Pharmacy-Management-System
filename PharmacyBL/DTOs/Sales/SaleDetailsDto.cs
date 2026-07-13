using System;
using System.Collections.Generic;

namespace PharmacyBL.DTOs.Sales
{
    public class SaleDetailsDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string CashierName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<SaleItemViewDto> Items { get; set; } = new();
    }
}
