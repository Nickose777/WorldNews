using System.Data.Entity.ModelConfiguration;
using WorldNews.Core.Entities;

namespace WorldNews.Core.Configurations
{
    class CommentConfiguration : EntityTypeConfiguration<CommentEntity>
    {
        public CommentConfiguration()
        {
            this.HasKey(comment => comment.Id);

            this.Property(comment => comment.Text)
                .IsRequired()
                .HasMaxLength(200);

            this.HasRequired(comment => comment.Author)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.AuthorId);
            this.HasRequired(comment => comment.Article)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.ArticleId);

            this.HasOptional(comment => comment.ModeratorWhoBanned)
                .WithMany(user => user.BannedComments)
                .HasForeignKey(comment => comment.ModeratorWhoBannedId);
            this.HasOptional(comment => comment.BanReason)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.BanReasonId);
        }
    }
}
