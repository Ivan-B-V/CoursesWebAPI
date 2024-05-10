using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity;

internal sealed class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasKey(x => x.Id);

        builder.Property(x => x .ClaimType)
            .HasMaxLength(50)
            .IsFixedLength(false)
            .IsRequired();

        builder.Property(x => x.ClaimValue)
            .HasMaxLength(50)
            .IsFixedLength(false)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();
    }
}
