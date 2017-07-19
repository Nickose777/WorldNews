using WorldNews.Core.Entities;

namespace WorldNews.Data.Contracts.Repositories
{
    public interface IBanReasonRepository : IRepository<BanReasonEntity>
    {
        bool Exists(string name);
    }
}
