using System.Linq;
using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class CategoryRepository : RepositoryBase<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(WorldNewsDbContext context)
            : base(context) { }

        public bool Exists(string name)
        {
            return context.Categories.Any(category => category.Name == name);
        }

        public CategoryEntity GetByName(string categoryName)
        {
            return context.Categories.SingleOrDefault(category => category.Name == categoryName);
        }
    }
}
