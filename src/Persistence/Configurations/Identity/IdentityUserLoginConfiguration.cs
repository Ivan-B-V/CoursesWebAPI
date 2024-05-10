using CoursesWebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity;

internal sealed class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.ToTable("UserLogins");

        builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
        
        builder.Property(x => x.LoginProvider)
            .HasMaxLength(128)
            .IsFixedLength(false)
            .IsRequired();
        
        builder.Property(x => x.ProviderKey)
            .HasMaxLength(128)
            .IsFixedLength(false)
            .IsRequired();

        builder.Property(x => x.ProviderDisplayName)
           .HasMaxLength(128)
           .IsFixedLength(false)
           .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();
    }
}
