using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(WorldNewsDbContext context)
            : base(context) { }
    }
}
