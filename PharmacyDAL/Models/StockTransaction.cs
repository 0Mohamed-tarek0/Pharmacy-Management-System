using System;
using PharmacyDAL.Enums;

namespace PharmacyDAL.Models
{
    /// <summary>
    /// An audit trail row for every stock movement (purchase, sale, return, adjustment).
    /// Quantity is stored in the Medicine's base unit and is signed:
    /// positive increases stock (Purchase/Return), negative decreases it (Sale).
    /// </summary>
    public class StockTransaction
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int? MedicineBatchId { get; set; }
        public MedicineBatch MedicineBatch { get; set; }

        public StockTransactionType Type { get; set; }

        /// <summary>Signed quantity in base units.</summary>
        public int Quantity { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        /// <summary>"Order" or "Sale" - the source document type.</summary>
        public string? ReferenceType { get; set; }

        /// <summary>Id of the Order/Sale that generated this transaction.</summary>
        public int? ReferenceId { get; set; }

        public string? Notes { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
