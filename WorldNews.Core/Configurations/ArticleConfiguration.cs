using System.Data.Entity.ModelConfiguration;
using WorldNews.Core.Entities;

namespace WorldNews.Core.Configurations
{
    class ArticleConfiguration : EntityTypeConfiguration<ArticleEntity>
    {
        public ArticleConfiguration()
        {
            this.HasKey(article => article.Id);

            this.Property(article => article.Header)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(article => article.ShortDescription)
                .IsRequired()
                .HasMaxLength(200);
            this.Property(article => article.PhotoLink)
                .IsRequired()
                .HasMaxLength(200);
            this.Property(article => article.Text)
                .IsRequired();

            this.HasRequired(article => article.Category)
                .WithMany(category => category.Articles)
                .HasForeignKey(article => article.CategoryId);
            this.HasRequired(article => article.Author)
                .WithMany(author => author.Articles)
                .HasForeignKey(article => article.AuthorId);
        }
    }
}
