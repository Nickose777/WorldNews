using WorldNews.Core.Entities;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface ICategoryRepository : IRepository<CategoryEntity>
    {
        bool Exists(string name);
    }
}
