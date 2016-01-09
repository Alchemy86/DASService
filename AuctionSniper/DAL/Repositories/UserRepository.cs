using System;
using System.Linq;
using DAS.Domain;
using DAS.Domain.DeathbyCapture;
using DAS.Domain.GoDaddy.Users;
using DAS.Domain.Users;

namespace DAL.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            if (unitOfWork == null) { throw new NotImplementedException("IUnitOfWork"); }
        }
            
        private DeathByCaptureDetails GetDeathByCaptureDetailsDetails()
        {
            var username = Context.SystemConfig.First(x => x.PropertyID == "DBCUser").Value;
            var password = Context.SystemConfig.First(x => x.PropertyID == "DBCPass").Value;

            return new DeathByCaptureDetails
            {
                Password = password,
                Username = username
            };
        }

        public GoDaddySession GetSessionDetails(string username)
        {
            var details = Context.Users.Include("GoDaddyAccount").FirstOrDefault(x => x.Username == username);
            if (details == null) return null;
            var gdAccount = details.GoDaddyAccount.FirstOrDefault() != null
                ? details.GoDaddyAccount.First().ToDomainObject()
                : null;
            return new GoDaddySession(details.Username, details.Password, gdAccount, GetDeathByCaptureDetailsDetails());
        }
    }
}
