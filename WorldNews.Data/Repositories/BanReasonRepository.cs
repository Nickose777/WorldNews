using System.Linq;
using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class BanReasonRepository : RepositoryBase<BanReasonEntity>, IBanReasonRepository
    {
        public BanReasonRepository(WorldNewsDbContext context)
            : base(context) { }

        public bool Exists(string name)
        {
            return context.BanReasons.Any(ban => ban.Name == name);
        }
    }
}
