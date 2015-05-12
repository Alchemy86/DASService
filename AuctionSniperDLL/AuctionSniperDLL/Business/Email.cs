using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using ASEntityFramework;

namespace AuctionSniperDLL.Business
{
    public static class Email
    {   
        private static SmtpClient SmtpClient()
        {
            SmtpClient client;
            using (var ds = new ASEntities())
            {
                var smtp = ds.SystemConfig.First(x => x.PropertyID == "ServiceEmailSMTP").Value;
                var port = ds.SystemConfig.First(x => x.PropertyID == "ServiceEmailPort").Value;
                var user = ds.SystemConfig.First(x => x.PropertyID == "ServiceEmailUser").Value;
                var password = ds.SystemConfig.First(x => x.PropertyID == "ServiceEmailPass").Value;

                client = new SmtpClient(smtp, int.Parse(port))
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(user, password)
                };
            }

            return client;
        }

        public static void SendEmail(string to,string subject, string message)
        {
            try
            {
                using (var ds = new ASEntities())
                {
                    var email = ds.SystemConfig.First(x => x.PropertyID == "ServiceEmail").Value;
                    var mail = new MailMessage(email, to)
                    {
                        Subject = subject,
                        Body = message
                    };

                    SmtpClient().Send(mail);
                }
            }
            catch (Exception ex)
            {
                new Error().Add("Failed to send email: " + ex.Message);
            }

        }
    }
}
