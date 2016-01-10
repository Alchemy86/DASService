namespace DAL
{
    public partial class GoDaddyAccount
    {
        public DAS.Domain.GoDaddy.Users.GoDaddyAccount ToDomainObject()
        {
            return new DAS.Domain.GoDaddy.Users.GoDaddyAccount
            {
                AccountId = AccountID,
                Username = GoDaddyUsername,
                Password = GoDaddyPassword,
                Verified = Verified,
                AccountUsername = Users.Username,
                ReceiveEmail = Users.ReceiveEmails
            };
        }
    }
}
