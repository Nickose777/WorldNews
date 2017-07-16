using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using WorldNews.Core;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly WorldNewsDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public RepositoryBase(WorldNewsDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public virtual TEntity Get(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return dbSet.Where(expression).ToList();
        }
    }
}
