using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Common.Helpers
{
    public class MailHelper
    {
        public const string From = "mmanojlovic481@gmail.com";
        public const string Password = "lktk nohg yiao ccda";
        public static void SendMail(string subject, string body, string to)
        {
            try
            {

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(From),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
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
            catch (System.Exception e)
            {
                Trace.TraceError(e.Message);
            }
        }
    }
}
