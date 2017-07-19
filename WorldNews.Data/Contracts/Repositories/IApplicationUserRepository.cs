using WorldNews.Core.Entities;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser Get(string id);

        ApplicationUser GetByLogin(string login);
    }
}
