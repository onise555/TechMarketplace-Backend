//using System.Net;
//using System.Net.Mail;

//namespace TechMarketplace.API.SMTP
//{
//    public class EmailSender
//    {
//        public void SendMail(string to, string subject, string body)
//        {
//            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
//            smtpClient.EnableSsl = true;
//            smtpClient.UseDefaultCredentials = false;

//            smtpClient.Credentials = new NetworkCredential("tsotskhalashvili558@gmail.com", "wfit myhi rnml nwmz");
//            MailMessage message = new MailMessage();
//            message.From = new MailAddress("tsotskhalashvili558@gmail.com");
//            message.To.Add(to);
//            message.Subject = subject;
//            message.Body = body;
//            message.IsBodyHtml = true;

//            smtpClient.Send(message);
//        }

//    }
//}
using System.Net;
using System.Net.Mail;

namespace TechMarketplace.API.SMTP
{
    public class EmailSender
    {
        public void SendMail(string to, string subject, string body)
        {
            // 1. ეს ხაზი აუცილებელია Railway/Linux-ისთვის SSL სერტიფიკატის ნდობისთვის
            ServicePointManager.ServerCertificateValidationCallback = (s, c, h, e) => true;

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 465))
            {
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("tsotskhalashvili558@gmail.com", "wfit myhi rnml nwmz");

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;


                // 2. დავამატოთ თაიმაუტი (5 წამი), რომ Swagger არ გაიჭედოს უსასრულოდ
                smtpClient.Timeout = 5000;

                MailMessage message = new MailMessage();
                message.From = new MailAddress("tsotskhalashvili558@gmail.com");
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                try
                {
                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    // თუ იმეილი ვერ წავა, ბაზაში მონაცემი მაინც დარჩება და ლოგებში ვნახავთ მიზეზს
                    Console.WriteLine($"SMTP Error: {ex.Message}");
                }
            }
        }
    }
}