using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using web_api_sample.api.Models.Dtos;
using web_api_sample.api.Services.Interfaces;

namespace web_api_sample.api.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;

        public EmailService(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task SendAsync(EmailDto email)
        {
            MailMessage mail = EmailSetup(email);

            using SmtpClient smtp = new SmtpClient(_emailOptions.PrimaryDomain, _emailOptions.PrimaryPort);
            smtp.Credentials = new NetworkCredential(_emailOptions.UsernameEmail, _emailOptions.UsernamePassword);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);
        }

        private MailMessage EmailSetup(EmailDto email)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_emailOptions.UsernameEmail, "web-api-sample");
            mail.To.Add(new MailAddress(email.To));
            mail.Subject = $"[web-api-sample] - { email.Subject }";
            mail.Body = email.Body;
            //Settings
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            return mail;
        }
    }
}