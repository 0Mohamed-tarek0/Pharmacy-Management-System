using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PharmacyDAL.Models;

namespace PharmacyAPI.Auth
{
    // This class replaces what the MVC cookie sign-in used to do.
    // Instead of writing an auth cookie, it builds a signed JWT that the
    // client stores and sends back in the Authorization header.
    public class JwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GenerateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var jwtSection = _config.GetSection("Jwt");
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.FullName ?? user.UserName ?? string.Empty)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryMinutes = int.Parse(jwtSection["ExpiryMinutes"] ?? "120");

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
