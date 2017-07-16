using WorldNews.Core;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts.Repositories;

namespace WorldNews.Data.Repositories
{
    class CommentRepository : RepositoryBase<CommentEntity>, ICommentRepository
    {
        public CommentRepository(WorldNewsDbContext context)
            : base(context) { }
    }
}
