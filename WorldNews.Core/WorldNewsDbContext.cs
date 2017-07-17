using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WorldNews.Core.Configurations;
using WorldNews.Core.Entities;

namespace WorldNews.Core
{
    public class WorldNewsDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public DbSet<ArticleEntity> Articles { get; set; }

        public DbSet<BanReasonEntity> BanReasons { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<CommentEntity> Comments { get; set; }

        public DbSet<ModeratorEntity> Moderators { get; set; }

        public DbSet<UserEntity> PortalUsers { get; set; }

        public WorldNewsDbContext()
            : base("DefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<WorldNewsDbContext>());

            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new ArticleConfiguration());
            modelBuilder.Configurations.Add(new BanReasonConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new ModeratorConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
