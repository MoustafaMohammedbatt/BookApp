using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDTO model, string role);
        Task<SignInResult> LoginUserAsync(LoginDTO model);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<bool> IsUserInRoleAsync(AppUser user, string role);
    }

}
