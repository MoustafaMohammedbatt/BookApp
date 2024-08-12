using Domain.Consts;
using Domain.Entites;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Persistence.Helpers
{
    public class DbInitializer
    {
        public static async Task Seed(IApplicationBuilder app, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            ApplicationDbContext context = app.ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                await roleManager.CreateAsync(new IdentityRole(UserRole.User));
                await roleManager.CreateAsync(new IdentityRole(UserRole.Reciptionist));
            }
            if (!userManager.Users.Any())
            {
                AppUser admin = new()
                {
                    FirstName = "admin",
                    LastName = "user",
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    Address = "test address",
                    EmailConfirmed = true,
                    AdminAccepted = true,
                    PasswordHash = "123123123@Zftt"
                };

                var user = await userManager.FindByEmailAsync(admin.Email);
                if (user is null)
                {
                    await userManager.CreateAsync(admin, "123123123@Zftt");
                    await userManager.AddToRoleAsync(admin, UserRole.Admin);
                }
            }
        }

    }
}
