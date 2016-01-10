using System;

namespace DAS.Domain
{
    public interface IEmail
    {
        void SendEmail(string to, string subject, string message, DateTime TimeStamp);
    }
}