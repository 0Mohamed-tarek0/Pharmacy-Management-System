using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Returns
{
    public class ReturnRecordDto
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        /// <summary>"Purchase" (returned to supplier) or "Sale" (returned by customer).</summary>
        public string ReturnType { get; set; } = string.Empty;

        /// <summary>The Order's OrderNumber or the Sale's InvoiceNumber.</summary>
        public string DocumentNumber { get; set; } = string.Empty;

        /// <summary>Id of the source Order or Sale, for linking back to its Details page.</summary>
        public int DocumentId { get; set; }

        public string MedicineName { get; set; } = string.Empty;

        public string BatchNumber { get; set; } = string.Empty;

        /// <summary>Quantity returned (always positive, in the medicine's base unit).</summary>
        public int Quantity { get; set; }

        public string? Notes { get; set; }

        public string PerformedBy { get; set; } = string.Empty;
    }
}
