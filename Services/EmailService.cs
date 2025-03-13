using electro_shop_backend.Configurations;
using electro_shop_backend.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace electro_shop_backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MailMessage
            {
                From = new MailAddress(_emailConfig.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            email.To.Add(to);

            using var smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
            {
                Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password),
                EnableSsl = true
            };
            await smtp.SendMailAsync(email);
        }
    }
}
