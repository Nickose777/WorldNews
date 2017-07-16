using System.Data.Entity.ModelConfiguration;
using WorldNews.Core.Entities;

namespace WorldNews.Core.Configurations
{
    class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            this.HasKey(appUser => appUser.Id);

            this.Property(appUser => appUser.UserName)
                .IsRequired()
                .HasMaxLength(30);
            this.Property(appUser => appUser.Email)
                .IsRequired()
                .HasMaxLength(30);
            this.Property(appUser => appUser.FirstName)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(appUser => appUser.LastName)
                .IsRequired()
                .HasMaxLength(20);

            this.HasOptional(appUser => appUser.User)
                .WithRequired(user => user.User);
            this.HasOptional(appUser => appUser.Moderator)
                .WithRequired(user => user.User);
        }
    }
}
