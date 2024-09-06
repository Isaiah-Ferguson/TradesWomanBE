using System.Net.Mail;
using System.Net;

namespace TradesWomanBE.Services
{
    public class EmailServices
    {
        private readonly string _smtpServer = "your-smtp-server";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "your-username";
        private readonly string _smtpPassword = "your-password";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress("your-email@example.com");
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    await client.SendMailAsync(message);
                }
            }
        }
    }
}