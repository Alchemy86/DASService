using DAS.Domain.GoDaddy.Users;

namespace DAS.Domain.Users
{
    public interface IUserRepository
    {
        GoDaddySession GetSessionDetails(string username);

    }
}