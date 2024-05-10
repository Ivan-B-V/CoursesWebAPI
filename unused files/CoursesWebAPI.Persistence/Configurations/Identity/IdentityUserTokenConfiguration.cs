using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity
{
    internal sealed class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        {
            builder.ToTable("UserTokens");
            
            builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

            builder.Property(x => x.LoginProvider)
                .HasMaxLength(128)
                .IsFixedLength(false)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(128)
                .IsFixedLength(false)
                .IsRequired();

            builder.Property(x => x.Value)
                .HasMaxLength(128)
                .IsFixedLength(false)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();
        }
    }
}
