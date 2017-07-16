using System.Data.Entity.ModelConfiguration;
using WorldNews.Core.Entities;

namespace WorldNews.Core.Configurations
{
    class BanReasonConfiguration : EntityTypeConfiguration<BanReasonEntity>
    {
        public BanReasonConfiguration()
        {
            this.HasKey(ban => ban.Id);

            this.Property(ban => ban.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
