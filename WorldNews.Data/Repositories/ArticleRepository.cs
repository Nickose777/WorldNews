using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class ArticleRepository : RepositoryBase<ArticleEntity>, IArticleRepository
    {
        public ArticleRepository(WorldNewsDbContext context)
            : base(context) { }
    }
}
