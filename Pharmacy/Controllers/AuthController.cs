using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.ViewModels.Authentication;
using PharmacyBL.DTOs.Authentication;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new LoginDto
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            if (await _authService.LoginAsync(dto))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid email or password.");

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
