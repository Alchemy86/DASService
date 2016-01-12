using System;
namespace DAS.Domain.GoDaddy.Users
{
    public class GoDaddyAccount
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Verified { get; set; }
        public Guid UserID { get; set; }

        public string AccountUsername { get; set; }
        public bool ReceiveEmail { get; set; }
    }
}
