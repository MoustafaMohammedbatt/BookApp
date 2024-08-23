using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Domain.Entites;
using System.Text.Encodings.Web;

namespace BookApp.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private const string TemplatePath = "wwwroot/email-templates/ConfirmEmail.html";

        public ConfirmEmailModel(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            StatusMessage = string.Empty;
        }

        [TempData]
        public string StatusMessage { get; set; }

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
            StatusMessage = "Thank you for confirming your email.";

            if (result.Succeeded)
            {
                var confirmationLink = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = user.Id, code },
                    protocol: Request.Scheme);

                var emailBody = await GetEmailBodyAsync(confirmationLink!);
                await _emailSender.SendEmailAsync(user.Email!, "Confirm your email", emailBody);
            }

            return Page();
        }

        private async Task<string> GetEmailBodyAsync(string confirmationLink)
        {
            var template = await System.IO.File.ReadAllTextAsync(TemplatePath);
            return template.Replace("{ConfirmationLink}", HtmlEncoder.Default.Encode(confirmationLink));
        }
    }
}
