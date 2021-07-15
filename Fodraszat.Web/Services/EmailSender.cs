using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Fodraszat.Web.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Fodraszat.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSettings EmailSettings { get; }

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            EmailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(EmailSettings.Host, EmailSettings.Port)
            {
                Credentials = new NetworkCredential(EmailSettings.Mail, EmailSettings.Password),
                EnableSsl = true
            };

            await client.SendMailAsync(new MailMessage(EmailSettings.Mail, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            });
        }
    }
}
