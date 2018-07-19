using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace web_pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder BodyBuilder = new StringBuilder();
            foreach (string Url in AppConfig.UrlsToPing)
            {
                if (IsUrlValid(Url))
                {
                    BodyBuilder.AppendLine(Url);
                }
            }
            SendEmail(BodyBuilder.ToString());
        }

        private static bool IsUrlValid(string Url)
        {
            WebRequest WebRequest = WebRequest.Create(Url);
            WebResponse WebResponse;
            try
            {
                WebResponse = WebRequest.GetResponse();
            }
            catch // If exception thrown then couldn't get response from address
            {
                return false;
            }
            return true;
        }

        private static void SendEmail(string Body)
        {
            string fromEmail = AppConfig.EmailRecipient;
            MailMessage mailMessage = new MailMessage(fromEmail, fromEmail, "Your subject here", Body);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromEmail, "YOUR PASSWORD HERE. I SUGGEST USING THROWAY GMAIL ACCOUNT");
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                //Error
                Console.WriteLine(ex.Message);
            }
        }
    }
}
