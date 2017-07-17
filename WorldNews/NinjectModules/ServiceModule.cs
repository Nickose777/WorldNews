using Ninject.Modules;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.Services;

namespace WorldNews.NinjectModules
{
    class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IAccountService>().To<AccountService>();
            this.Bind<IProfileService>().To<ProfileService>();
            this.Bind<IModeratorService>().To<ModeratorService>();
            this.Bind<ICategoryService>().To<CategoryService>();
        }
    }
}