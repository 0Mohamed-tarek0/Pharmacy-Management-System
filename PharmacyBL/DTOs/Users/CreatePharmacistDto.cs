using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Users
{
    public class CreatePharmacistDto
    {
        public string FullName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
    }
}
