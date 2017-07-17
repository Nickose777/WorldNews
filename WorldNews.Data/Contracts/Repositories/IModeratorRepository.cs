using WorldNews.Core.Entities;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface IModeratorRepository : IRepository<ModeratorEntity>
    {
        ModeratorEntity GetByLogin(string login);
    }
}
