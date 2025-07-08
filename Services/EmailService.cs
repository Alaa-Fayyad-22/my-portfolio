using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string fromName, string fromEmail, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderName"]),
            Subject = $"New Contact Message from {fromName}",
            Body = $"From: {fromName} ({fromEmail})\n\n{message}",
            IsBodyHtml = false
        };

        // Receiver: your business email
        mailMessage.To.Add(_config["EmailSettings:ReceiverEmail"]);

        // Reply-To: user who sent the message
        mailMessage.ReplyToList.Add(new MailAddress(fromEmail));

        // Validate and add ReplyTo only if fromEmail is valid
        if (!string.IsNullOrWhiteSpace(fromEmail))
        {
            try
            {
                mailMessage.ReplyToList.Add(new MailAddress(fromEmail));
            }
            catch (FormatException)
            {
                // Optionally handle invalid email format here
                throw new ArgumentException("Invalid sender email format.");
            }
        }

        // Validate and add To address
        string toEmail = _config["EmailSettings:ReceiverEmail"]; // or SenderEmail if same
        if (string.IsNullOrWhiteSpace(toEmail))
        {
            throw new ArgumentException("Receiver email address is not configured.");
        }
        mailMessage.To.Add(toEmail);

        var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]),
            Credentials = new NetworkCredential(
                _config["EmailSettings:Username"],
                _config["EmailSettings:Password"]
            ),
            EnableSsl = true
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}
