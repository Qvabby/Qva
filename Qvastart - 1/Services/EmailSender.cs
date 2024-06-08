using Qvastart___1.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Qvastart___1.Services
{
    public class EmailSender : ICustomEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration con) { _configuration = con; }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            //Getting Credentials
            var options = _configuration.GetSection("Credentials").Get<EmailSenderOptions>();
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(options.Email, options.Password)
            };
            var MailMessage = new MailMessage(from: options.Email, to: email, subject, message);
            MailMessage.IsBodyHtml = true;
            MailMessage.Subject = "OFFICIAL QVASTORE WEBSITE | " + subject;
            //sending Mail.
            return client.SendMailAsync(MailMessage);
        }
    }
}
