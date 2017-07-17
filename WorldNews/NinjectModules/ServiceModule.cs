using Ninject.Modules;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Services;

namespace WorldNews.NinjectModules
{
    class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IAccountService>().To<AccountService>();
            this.Bind<IProfileService>().To<ProfileService>();
        }
    }
}