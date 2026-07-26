using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Auth;
using PharmacyDAL.Models;

namespace PharmacyAPI.Controllers
{
    // DTO for the login request body. In the MVC app this was a LoginViewModel
    // bound to a Razor form; here it's just the JSON body of a POST.
    public record LoginRequest(string Email, string Password);

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, JwtTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // POST api/auth/login
        // MVC's AuthController called _authService.LoginAsync(), which used
        // SignInManager to write an auth cookie and then redirected to a page.
        // An API has no pages to redirect to and no cookie to write - it just
        // checks the password and hands back a JWT as JSON.
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized(new { message = "Invalid email or password." });

            var token = await _tokenService.GenerateTokenAsync(user, _userManager);

            return Ok(new
            {
                token,
                userId = user.Id,
                fullName = user.FullName,
                roles = await _userManager.GetRolesAsync(user)
            });
        }

        // POST api/auth/logout
        // There is no server-side session or cookie to clear for a JWT API -
        // "logging out" just means the client discards the token it's holding.
        // This endpoint is kept only so a client has something to call.
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out. Discard the token on the client." });
        }
    }
}
