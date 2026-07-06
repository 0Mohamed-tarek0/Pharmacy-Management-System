using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace PharmacyDAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        // Navigation properties
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
