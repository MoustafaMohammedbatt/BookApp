using Domain.Consts;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions.Interfaces.IServises;
using Shared.DTOs;

namespace BookApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
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
                var result = await _authService.RegisterUserAsync(model, UserRole.Admin);
                if (result.Succeeded)
                {
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
                var result = await _authService.RegisterUserAsync(model, UserRole.Reciptionist);
                if (result.Succeeded)
                {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginUserAsync(model);
                if (result.Succeeded)
                {
                    var user = await _authService.GetUserByEmailAsync(model.Email);
                    if (await _authService.IsUserInRoleAsync(user, UserRole.Admin))
                    {
                        ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (await _authService.IsUserInRoleAsync(user, UserRole.Reciptionist))
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
