using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Authentication
{
    public class CreateUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; }
        public string Role { get; set; }
    }
}
