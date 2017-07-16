using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void Remove(TEntity entity);

        TEntity Get(int id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
    }
}
