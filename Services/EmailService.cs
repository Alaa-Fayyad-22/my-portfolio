using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyPortfolio.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string userName, string userEmail, string message)
        {
            var settings = _config.GetSection("EmailSettings");

            using (var client = new SmtpClient(settings["SmtpServer"], int.Parse(settings["Port"])))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(settings["Username"], settings["Password"]);

                var mail = new MailMessage
                {
                    From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                    Subject = $"New Contact Message from {userName}",
                    Body = $"Sender: {userName} ({userEmail})\n\nMessage:\n{message}",
                    IsBodyHtml = false,
                };

                mail.ReplyToList.Add(new MailAddress(userEmail, userName));
                mail.To.Add(settings["ReceiverEmail"]);

                await client.SendMailAsync(mail);
            }
        }
    }
}
