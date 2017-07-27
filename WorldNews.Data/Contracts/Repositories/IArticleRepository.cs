using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WorldNews.Core.Entities;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface IArticleRepository : IRepository<ArticleEntity>
    {
        int Count();

        int Count(Expression<Func<ArticleEntity, bool>> expression);
    }
}
