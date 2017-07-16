using WorldNews.Core.Entities;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        bool EmailExists(string login);

        bool LoginExists(string email);
    }
}
