using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldNews.Core.Entities;

namespace WorldNews.Logic.Identity
{
    class RoleManager : RoleManager<ApplicationRole>
    {
        public RoleManager(IRoleStore<ApplicationRole, string> store)
            : base(store) { }
    }
}
