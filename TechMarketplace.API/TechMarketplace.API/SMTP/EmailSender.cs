using System.Net;
using System.Net.Mail;

namespace TechMarketplace.API.SMTP
{
    public class EmailSender
    {
        public void SendMail(string to, string subject, string body)
        {
            // ეს ხაზი აუცილებელია Railway/Linux-ისთვის
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, c, h, e) => true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("tsotskhalashvili558@gmail.com", "wfit myhi rnml nwmz");

            MailMessage message = new MailMessage();
            message.From = new MailAddress("tsotskhalashvili558@gmail.com");
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            smtpClient.Send(message);
        }
    }
    
}
