using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Domain.Entites;
using System.Text.Encodings.Web;

namespace BookApp.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private const string TemplatePath = "wwwroot/email-templates/ForgotPasswordEmail.html";

        public ForgotPasswordModel(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            Input = new InputModel(); // Initialize the Input property
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var emailBody = await GetEmailBodyAsync(callbackUrl!);
                await _emailSender.SendEmailAsync(Input.Email, "Reset Password", emailBody);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        private async Task<string> GetEmailBodyAsync(string resetLink)
        {
            var template = await System.IO.File.ReadAllTextAsync(TemplatePath);
            return template.Replace("{ResetLink}", HtmlEncoder.Default.Encode(resetLink));
        }
    }
}
