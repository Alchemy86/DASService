using System;
using System.Net;
using System.Net.Mail;

namespace AuctionSniperDLL.Business
{
    public static class Email
    {
        public static void SendEmail(string to,string subject, string message)
        {
            try
            {
                var mail = new MailMessage("dasservicereport@gmail.com", to);
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("dasservicereport@gmail.com", "Pr10r1tyF1x")
                };
                mail.Subject = subject;
                mail.Body = message;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Failed to send email");
            }

        }
    }
}
