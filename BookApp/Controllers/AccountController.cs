using AutoMapper;
using Domain.Consts;
using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace BookApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        [HttpGet(UserRole.Admin)]
        public IActionResult RegisterAsAdmin()
        {
            return View();
        }

        [HttpPost(UserRole.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsAdmin(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                // Check if the Admin role exists, if not, create it
                if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                }

                var user = _mapper.Map<AppUser>(model);
                user.UserName = model.Email;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign the user to the Admin role
                    await _userManager.AddToRoleAsync(user, UserRole.Admin);

                    // Redirect to login page instead of signing in the user
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet(UserRole.Reciptionist)]
        public IActionResult RegisterAsReciptionist()
        {
            return View();
        }

        [HttpPost(UserRole.Reciptionist)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsReciptionist(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                // Check if the Admin role exists, if not, create it
                if (!await _roleManager.RoleExistsAsync(UserRole.Reciptionist))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRole.Reciptionist));
                }

                var user = _mapper.Map<AppUser>(model);
                user.UserName = model.Email;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign the user to the Admin role
                    await _userManager.AddToRoleAsync(user, UserRole.Reciptionist);

                    // Redirect to login page instead of signing in the user
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


    }
}
