using System;
using System.Collections.Generic;

namespace PharmacyDAL.Models
{
    /// <summary>
    /// Represents one physical batch/lot of a Medicine.
    /// The same Medicine can have many batches, each with its own
    /// batch number, expiry date, purchase/selling price and quantity.
    /// Quantity is always stored in the Medicine's base (smallest sellable) unit.
    /// </summary>
    public class MedicineBatch
    {
        public int Id { get; set; }

        // Foreign Key -> Medicine
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public bool IsActive { get; set; } = true;
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }
        /// <summary>
        /// Quantity on hand for this batch, expressed in the Medicine's base unit
        /// (e.g. Strip, Ampoule, Tube, Bottle...).
        /// </summary>
        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}
