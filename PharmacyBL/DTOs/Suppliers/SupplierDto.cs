using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Suppliers
{
    public class SupplierDto
    {
        public int Id { get; set; }

        public string CompanyName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
