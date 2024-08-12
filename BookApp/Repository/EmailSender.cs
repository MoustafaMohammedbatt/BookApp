using Microsoft.Extensions.Options;
using Persistence.Helpers;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace BookApp.Repository
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings mailSettings;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmailSender(IOptions<MailSettings> mailSettings, IWebHostEnvironment webHostEnvironment)
        {
            this.mailSettings = mailSettings?.Value ?? throw new ArgumentNullException(nameof(mailSettings));
            this.webHostEnvironment = webHostEnvironment;

            if (string.IsNullOrEmpty(this.mailSettings.Email))
            {
                throw new ArgumentException("Email is not configured in mail settings.");
            }

            if (string.IsNullOrEmpty(this.mailSettings.DisplayName))
            {
                throw new ArgumentException("DisplayName is not configured in mail settings.");
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(email))
                return;

            var message = new MailMessage
            {
                From = new MailAddress(mailSettings.Email, mailSettings.DisplayName),
                Subject = subject,
                Body = $"<html><body>{htmlMessage}</body></html>",
                IsBodyHtml = true
            };
            message.To.Add(email);

            using var smtpClient = new SmtpClient(mailSettings.Host, mailSettings.Port)
            {
                Credentials = new NetworkCredential(mailSettings.Email, mailSettings.Password),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);
        }

    }

}
