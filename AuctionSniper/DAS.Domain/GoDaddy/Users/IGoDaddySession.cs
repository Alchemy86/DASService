using DAS.Domain.DeathbyCapture;

namespace DAS.Domain.GoDaddy.Users
{
    public interface IGoDaddySession
    {
        bool LoggedIn { get; set; }
        string Username { get; }
        string Password { get; }
        GoDaddyAccount GoDaddyAccount { get; }
        DeathByCaptureDetails DeathByCapture { get; }
    }
}