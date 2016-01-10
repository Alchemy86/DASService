using System;
using System.Net;
using System.Net.Mail;

namespace DAS.Domain
{
    public class Email : IEmail
    {
        private readonly ISystemRepository _systemProperSystemRepository;
        public Email(ISystemRepository repository)
        {
            _systemProperSystemRepository = repository;
        }

        private SmtpClient SmtpClient()
        {
            var smtp = _systemProperSystemRepository.ServiceEmailSmtp;
            var port = _systemProperSystemRepository.ServiceEmailPort;
            var user = _systemProperSystemRepository.ServiceEmailUser;
            var password = _systemProperSystemRepository.ServiceEmailPassword;

            var client = new SmtpClient(smtp, int.Parse(port))
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, password)
            };

            return client;
        }

        public void SendEmail(string to, string subject, string message, DateTime TimeStamp)
        {
            try
            {
                var email = _systemProperSystemRepository.ServiceEmail;
                var mail = new MailMessage(email, to)
                {
                    Subject = subject,
                    Body = message + Environment.NewLine + TimeStamp.ToString("dd/M/yyyy H:mm:ss")
                };

                SmtpClient().Send(mail);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
