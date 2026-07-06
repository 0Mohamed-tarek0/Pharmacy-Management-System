using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyBL.Common;
using PharmacyBL.DTOs.Users;

namespace PharmacyBL.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceResult> CreatePharmacistAsync(CreatePharmacistDto dto);
    }
}
