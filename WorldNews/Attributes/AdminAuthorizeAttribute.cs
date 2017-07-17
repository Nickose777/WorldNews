using System.Web.Mvc;

namespace WorldNews.Attributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            Roles = WorldNews.Logic.Infrastructure.Roles.AdminRole;
        }
    }
}