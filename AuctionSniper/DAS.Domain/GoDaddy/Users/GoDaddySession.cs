using DAS.Domain.DeathbyCapture;

namespace DAS.Domain.GoDaddy.Users
{
    public class GoDaddySession : IGoDaddySession
    {
        public GoDaddySession(string username, string password, GoDaddyAccount goDaddyAccount, DeathByCaptureDetails deathByCapture)
        {
            Username = username;
            Password = password;
            GoDaddyAccount = goDaddyAccount;
            DeathByCapture = deathByCapture;
        }

        public bool LoggedIn { get; set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public GoDaddyAccount GoDaddyAccount { get; private set; }
        public DeathByCaptureDetails DeathByCapture { get; private set; }
    }
}
