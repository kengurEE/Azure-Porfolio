using System.Net;
using System.Net.Mail;

namespace Common.Helpers
{
    public class MailHelper
    {
        public const string From = "";
        public const string Password = "";
        public static void SendMail(string subject, string body, string to)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(From, Password),
                EnableSsl = true,
            };
            smtpClient.Send(mailMessage);
        }
    }
}
