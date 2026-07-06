using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PharmacyDAL.Models;

namespace PharmacyBL.Helpers.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Pharmacist"))
            {
                await roleManager.CreateAsync(new IdentityRole("Pharmacist"));
            }

            var admin = await userManager.FindByEmailAsync("admin@pharmacy.com");
            if (admin == null)
            {
                admin = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@pharmacy.com",
                    FullName = "System Administrator"
                };

                var result = await userManager.CreateAsync(
                    admin,
                    "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
