using System;
using WorldNews.Core;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        WorldNewsDbContext Context { get; }

        IArticleRepository Articles { get; }

        IBanReasonRepository Bans { get; }

        ICategoryRepository Categories { get; }

        ICommentRepository Comments { get; }

        IModeratorRepository Moderators { get; }

        IUserRepository Users { get; }
    }
}
