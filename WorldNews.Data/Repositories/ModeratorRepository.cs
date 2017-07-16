using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class ModeratorRepository : RepositoryBase<ModeratorEntity>, IModeratorRepository
    {
        public ModeratorRepository(WorldNewsDbContext context)
            : base(context) { }
    }
}
