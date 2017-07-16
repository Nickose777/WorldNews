using System.Data.Entity.ModelConfiguration;
using WorldNews.Core.Entities;

namespace WorldNews.Core.Configurations
{
    class ModeratorConfiguration : EntityTypeConfiguration<ModeratorEntity>
    {
        public ModeratorConfiguration()
        {
            this.HasKey(moderator => moderator.Id);

            this.Property(moderator => moderator.PhotoLink)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
