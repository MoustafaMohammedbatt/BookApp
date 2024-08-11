using Domain.Consts;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                AppUser admin = new()
                {
                    FirstName = "admin",
                    LastName = "user",
                    UserName = "admin",
                    Email = "admin@BookApp.com",
                    Address = "test address",
                    EmailConfirmed = true,
                    AdminAccepted = true,
                };

                var user = await userManager.FindByEmailAsync(admin.Email);
                if (user is null)
                {
                    await userManager.CreateAsync(admin, "Passw@rd12345678");
                    await userManager.AddToRoleAsync(admin, UserRole.Admin);
                }
            }
        }
    }
}
