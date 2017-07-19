using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldNews.Core.Entities;

namespace WorldNews.Logic.Identity
{
    class UserManager : UserManager<ApplicationUser, string>
    {
        public UserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
            this.PasswordValidator = new PasswordValidator
            {
                RequireDigit = true,
                RequiredLength = 6,
                RequireLowercase = true,
                RequireUppercase = true
            };

            this.UserLockoutEnabledByDefault = true;
        }
    }
}
