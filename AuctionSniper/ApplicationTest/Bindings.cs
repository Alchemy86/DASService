using System;
using DAL;
using DAL.Repositories;
using DAS.Domain;
using DAS.Domain.GoDaddy.Users;
using DAS.Domain.Users;
using Lunchboxweb.BaseFunctions;
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
            mockObject.Setup(x => x.Username).Returns("useremail");
            mockObject.Setup(x => x.Password).Returns("password");
            mockObject.Setup(x => x.GoDaddyAccount).Returns(
                new GoDaddyAccount
                {
                    AccountId = Guid.NewGuid(),
                    Username = "username",
                    Password = "password",
                    Verified = false
                });
            Bind<IGoDaddySession>().ToConstant(mockObject.Object);
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUnitOfWork>().To<ASEntities>();
            Bind<ITextManipulation>().To<TextManipulation>();
            Bind<ISystemRepository>().To<SystemRepository>();
        }
    }
}
