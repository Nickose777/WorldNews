using System.Linq;
using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(WorldNewsDbContext context)
            : base(context) { }

        public bool LoginExists(string login)
        {
            return context.Users.Any(user => user.UserName == login);
        }

        public bool EmailExists(string email)
        {
            return context.Users.Any(user => user.Email == email);
        }
    }
}
