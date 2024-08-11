using Domain.Consts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                await roleManager.CreateAsync(new IdentityRole(UserRole.User));
                await roleManager.CreateAsync(new IdentityRole(UserRole.Reciptionist));
            }
        }
    }
}
