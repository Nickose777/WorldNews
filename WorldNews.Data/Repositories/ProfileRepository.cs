using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    public class ProfileRepository : RepositoryBase<ApplicationUser>, IProfileRepository
    {
        public ProfileRepository(WorldNewsDbContext context)
            : base(context) { }

        public ApplicationUser FindByLogin(string login)
        {
            return context.Users.SingleOrDefault(user => user.UserName == login);
        }

        public bool IsInRole(string userId, string roleName)
        {
            bool isInRole = false;

            ApplicationUser applicationUser = context.Users.SingleOrDefault(user => user.Id == userId);
            ApplicationRole applicationRole = context.Roles.SingleOrDefault(role => role.Name == roleName);

            if (applicationUser != null && applicationRole != null)
            {
                isInRole = applicationUser.Roles.Select(r => r.RoleId).Contains(applicationRole.Id);
            }

            return isInRole;
        }
    }
}
