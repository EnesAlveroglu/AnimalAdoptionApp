using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace AnimalAdoptionApp.Services
{
    public class MailService : IMailService, IEmailSender // 🔥 İkisini de implement ettik
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["Email:Host"])
            {
                Port = int.Parse(_config["Email:Port"]!),
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]
                ),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("info@hayvansahiplendir.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

        // 🔁 Identity için bu metodu da yazdık:
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMailAsync(email, subject, htmlMessage); // Tek noktadan yürütüyoruz
        }
    }
}
