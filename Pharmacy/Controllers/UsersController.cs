using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.ViewModels.Users;
using PharmacyBL.DTOs.Users;
using PharmacyBL.Interfaces.Services;

namespace Pharmacy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult CreatePharmacist()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePharmacist(CreatePharmacistViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreatePharmacistDto
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userService.CreatePharmacistAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(CreatePharmacist));
        }
    }
}
