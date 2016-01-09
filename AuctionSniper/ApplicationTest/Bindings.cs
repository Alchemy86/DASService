using System;
using DAL;
using DAL.Repositories;
using DAS.Domain;
using DAS.Domain.GoDaddy.Users;
using DAS.Domain.Users;
using Moq;
using Ninject.Modules;
using GoDaddyAccount = DAS.Domain.GoDaddy.Users.GoDaddyAccount;

namespace ApplicationTest
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            var mockObject = new Mock<IGoDaddySession>();
            mockObject.Setup(x => x.Username).Returns("aarongioucash1@hotmail.com");
            mockObject.Setup(x => x.Password).Returns("elementals1");
            mockObject.Setup(x => x.GoDaddyAccount).Returns(
                new GoDaddyAccount
                {
                    AccountId = Guid.NewGuid(),
                    Username = "trevmedia",
                    Password = "J021092p!",
                    Verified = false
                });
            Bind<IGoDaddySession>().ToConstant(mockObject.Object);
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUnitOfWork>().To<ASEntities>();
        }
    }
}
