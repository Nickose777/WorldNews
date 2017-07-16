using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldNews.Core.Entities;

namespace WorldNews.Logic.Identity
{
    class SignInManager : SignInManager<ApplicationUser, string>
    {
        public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) { }
    }
}
