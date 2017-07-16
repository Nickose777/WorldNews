using WorldNews.Core;
using WorldNews.Data.Contracts;
using WorldNews.Data.Contracts.Repositories;
using WorldNews.Data.Repositories;

namespace WorldNews.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IArticleRepository articles;
        private IBanReasonRepository bans;
        private ICategoryRepository categories;
        private ICommentRepository comments;
        private IModeratorRepository moderators;
        private IUserRepository users;

        public WorldNewsDbContext Context { get; private set; }

        public IArticleRepository Articles
        {
            get { return articles ?? (articles = new ArticleRepository(Context)); }
        }

        public IBanReasonRepository Bans
        {
            get { return bans ?? (bans = new BanReasonRepository(Context)); }
        }

        public ICategoryRepository Categories
        {
            get { return categories ?? (categories = new CategoryRepository(Context)); }
        }

        public ICommentRepository Comments
        {
            get { return comments ?? (comments = new CommentRepository(Context)); }
        }

        public IModeratorRepository Moderators
        {
            get { return moderators ?? (moderators = new ModeratorRepository(Context)); }
        }

        public IUserRepository Users
        {
            get { return users ?? (users = new UserRepository(Context)); }
        }

        public UnitOfWork(WorldNewsDbContext context)
        {
            this.Context = context;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
