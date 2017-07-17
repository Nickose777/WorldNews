using Microsoft.AspNet.Identity.EntityFramework;
using WorldNews.Core.Entities;

namespace WorldNews.Logic.Identity
{
    class RoleStore : RoleStore<ApplicationRole, string, IdentityUserRole>
    {
        public RoleStore(IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim> context)
            : base(context) { }
    }
}
