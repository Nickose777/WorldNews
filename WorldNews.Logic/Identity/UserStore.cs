using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using WorldNews.Core.Entities;

namespace WorldNews.Logic.Identity
{
    class UserStore : UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, IUserStore<ApplicationUser>, IUserStore<ApplicationUser, string>, IDisposable
    {
        public UserStore(IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim> context)
            : base(context) { }
    }
}
