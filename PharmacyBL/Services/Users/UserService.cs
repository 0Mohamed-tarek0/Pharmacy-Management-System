using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PharmacyBL.Common;
using PharmacyBL.DTOs.Users;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;

namespace PharmacyBL.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ServiceResult> CreatePharmacistAsync(CreatePharmacistDto dto)
        {
            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            IdentityResult result =
                await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            await _userManager.AddToRoleAsync(user, "Pharmacist");

            return new ServiceResult
            {
                Success = true,
                Message = "Pharmacist created successfully."
            };
        }

    }
}
