using WorldNews.Core.Entities;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface IProfileRepository : IRepository<ApplicationUser>
    {
        ApplicationUser FindByLogin(string login);

        bool IsInRole(string userId, string role);
    }
}
