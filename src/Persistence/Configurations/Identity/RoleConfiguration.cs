using CoursesWebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
               .HasMaxLength(50)
               .IsFixedLength(false)
               .IsRequired();

        builder.Property(r => r.NormalizedName)
               .HasMaxLength(50)
               .IsFixedLength(false)
               .IsRequired();

        builder.Property(u => u.ConcurrencyStamp)
               .IsConcurrencyToken()
               .HasMaxLength(36)
               .IsFixedLength(true)
               .IsRequired(false);

        builder.HasIndex(r => r.Name)
               .IsUnique();

        builder.HasMany(x => x.Permissions)
               .WithMany();
    }
}
