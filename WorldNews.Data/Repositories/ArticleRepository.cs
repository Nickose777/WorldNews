using System;
using System.Linq;
using System.Linq.Expressions;
using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class ArticleRepository : RepositoryBase<ArticleEntity>, IArticleRepository
    {
        public ArticleRepository(WorldNewsDbContext context)
            : base(context) { }

        public int Count()
        {
            return context.Articles.Count();
        }

        public int Count(Expression<Func<ArticleEntity, bool>> expression)
        {
            return context.Articles.Count(expression);
        }
    }
}
