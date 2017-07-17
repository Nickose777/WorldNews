using System.Web.Mvc;

namespace WorldNews.Attributes
{
    public class ModeratorAuthorizeAttribute : AuthorizeAttribute
    {
        public ModeratorAuthorizeAttribute()
        {
            Roles = WorldNews.Logic.Infrastructure.Roles.ModeratorRole;
        }
    }
}