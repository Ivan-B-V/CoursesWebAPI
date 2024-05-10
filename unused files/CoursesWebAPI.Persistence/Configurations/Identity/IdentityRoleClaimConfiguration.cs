using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity
{
    internal sealed class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            builder.ToTable("RoleClaims");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.ClaimType)
                .HasMaxLength(50)
                .IsFixedLength(false)
                .IsRequired();

            builder.Property(x => x.ClaimValue)
                .HasMaxLength(50)
                .IsFixedLength(false)
                .IsRequired();

            builder.Property(x => x.RoleId)
                .IsRequired();
        }
    }
}
