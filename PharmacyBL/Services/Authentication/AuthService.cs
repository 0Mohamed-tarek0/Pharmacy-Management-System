using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PharmacyBL.DTOs.Authentication;
using PharmacyBL.Interfaces.Services;
using PharmacyDAL.Models;

namespace PharmacyBL.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            
            if(user ==null)
                return false;

            var result = await _signInManager.PasswordSignInAsync(
                        user,
                        dto.Password,
                        dto.RememberMe,
                        lockoutOnFailure: false);

            return result.Succeeded;
        }

        // logout method

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
