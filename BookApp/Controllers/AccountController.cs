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

        [HttpGet("RegisterAsAdmin")]
        public IActionResult RegisterAsAdmin()
        {
            return View();
        }

        [HttpPost("RegisterAsAdmin")]
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
                user.EmailConfirmed = true;
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

        [HttpGet("RegisterAsReciptionist")]
        public IActionResult RegisterAsReciptionist()
        {
            return View();
        }

        [HttpPost("RegisterAsReciptionist")]
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
                user.EmailConfirmed = true;
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /*  if (User.IsInRole("Admin"))
        {
            ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
        }
        else if (User.IsInRole("Receptionist"))
        {
            ViewData["Layout"] = "~/Views/Shared/_ReceptionistLayout.cshtml";
        }
        else
        {
            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml"; // Default layout
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (await _userManager.IsInRoleAsync(user, UserRole.Admin))
                    {
                        ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (await _userManager.IsInRoleAsync(user, UserRole.Reciptionist))
                    {
                        ViewData["Layout"] = "~/Views/Shared/_ReceptionistLayout.cshtml";
                        return RedirectToAction("Index", "Receptionist");
                    }
                    else
                    {
                        ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
                        return RedirectToAction("Index", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }
    }

}

