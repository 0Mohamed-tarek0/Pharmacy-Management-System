namespace PharmacyBL.DTOs.Sales
{
    public class SaleItemViewDto
    {
        public int Id { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        /// <summary>How much of this line has already been returned by the customer.</summary>
        public int ReturnedQuantity { get; set; }

        /// <summary>How much of this line can still be returned by the customer.</summary>
        public int ReturnableQuantity => Quantity - ReturnedQuantity;
    }
}
