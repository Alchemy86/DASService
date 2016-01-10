using DAL;
using DAL.Repositories;
using DAS.Domain;
using DAS.Domain.Users;
using Ninject.Modules;

namespace AuctionSniperWindowsService
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IEmail>().To<Email>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<ISystemRepository>().To<SystemRepository>();
            Bind<IUnitOfWork>().To<ASEntities>();
        }
    }
}
