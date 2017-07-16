using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorldNews.Startup))]
namespace WorldNews
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}