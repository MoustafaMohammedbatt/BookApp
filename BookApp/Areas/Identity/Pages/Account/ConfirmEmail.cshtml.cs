using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity;
using Domain.Entites;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookApp.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailModel(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public string StatusMessage { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return Page();
        }
    }
}
