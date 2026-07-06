using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDAL.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        // Foreign Key -> ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        // Navigation property
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
