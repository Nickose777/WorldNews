using Microsoft.Owin.Security;
using Ninject.Modules;
using Ninject.Web.Common;
using System.Web;

namespace WorldNews.NinjectModules
{
    class OwinModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IAuthenticationManager>()
                .ToMethod(c => HttpContext.Current.GetOwinContext().Authentication)
                .InRequestScope();
        }
    }
}