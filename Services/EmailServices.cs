using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace TradesWomanBE.Services
{
    public class EmailServices
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "jacob.dekok01@gmail.com";
        private readonly string _smtpPassword = "your-app-password";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_smtpUsername);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    client.EnableSsl = true; // Ensure SSL is enabled
                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
    