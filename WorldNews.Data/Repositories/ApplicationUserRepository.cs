using System;
using System.Linq;
using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(WorldNewsDbContext context)
            : base(context) { }

        public override ApplicationUser Get(int id)
        {
            throw new InvalidOperationException();
        }

        public ApplicationUser Get(string id)
        {
            return context.Users.SingleOrDefault(applicationUser => applicationUser.Id == id);
        }

        public ApplicationUser GetByLogin(string login)
        {
            return context.Users.SingleOrDefault(applicationUser => applicationUser.UserName == login);
        }
    }
}
