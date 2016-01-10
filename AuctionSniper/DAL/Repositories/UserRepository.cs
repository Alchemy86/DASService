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

        public GoDaddySessionModel GetSessionDetails(string username)
        {
            var details = Context.Users.Include("GoDaddyAccount").FirstOrDefault(x => x.Username == username);
            if (details == null) return null;
            var gdAccount = details.GoDaddyAccount.FirstOrDefault() != null
                ? details.GoDaddyAccount.First().ToDomainObject()
                : null;
            return new GoDaddySessionModel(details.Username, details.Password, gdAccount, GetDeathByCaptureDetailsDetails());
        }

        /// <summary>
        /// Add an error to the database
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public virtual void LogError(string message, string type = "Error")
        {
            Context.EventLog.Add(new EventLog
            {
                CreatedDate = DateTime.Now,
                Event = type,
                Message = message
            });
            Context.Save();
        }


        /// <summary>
        /// Adds a histroy record
        /// </summary>
        /// <param name="message">History record message</param>
        /// <param name="auctionLink">auction to link to</param>
        public void AddHistoryRecord(string message, Guid auctionLink)
        {
            Context.AuctionHistory.Add(new AuctionHistory
            {
                AuctionLink = auctionLink,
                CreatedDate = Global.GetPacificTime,
                HistoryID = Guid.NewGuid(),
                Text = message
            });
            Context.Save();
        }

    }
}
