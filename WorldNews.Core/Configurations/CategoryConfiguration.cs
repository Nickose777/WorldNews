using System.Data.Entity.ModelConfiguration;
using WorldNews.Core.Entities;

namespace WorldNews.Core.Configurations
{
    class CategoryConfiguration : EntityTypeConfiguration<CategoryEntity>
    {
        public CategoryConfiguration()
        {
            this.HasKey(category => category.Id);

            this.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
