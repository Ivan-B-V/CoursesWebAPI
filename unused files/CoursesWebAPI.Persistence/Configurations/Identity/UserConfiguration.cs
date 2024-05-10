using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            
            builder.HasMany(u => u.RefreshTokens);
            
            builder.HasMany(u => u.Roles);

            builder.Property(u => u.UserName)
                   .HasMaxLength(128)
                   .IsFixedLength(false)
                   .IsRequired();

            builder.Property(u => u.NormalizedUserName)
                   .HasMaxLength(128)
                   .IsFixedLength(false)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .HasMaxLength(128)
                   .IsFixedLength(false)
                   .IsRequired();

            builder.Property(u => u.EmailConfirmed)
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(u => u.NormalizedEmail)
                   .HasMaxLength(128)
                   .IsFixedLength(false)
                   .IsRequired();

            builder.Property(u => u.PhoneNumber)
                   .HasMaxLength(14)
                   .IsFixedLength(false)
                   .IsRequired(false);

            builder.Property(u => u.PhoneNumberConfirmed)
                    .HasDefaultValue(false)
                    .IsRequired();

            builder.Property(u => u.SecurityStamp)
                   .HasMaxLength(36)
                   .IsFixedLength(false)
                   .IsRequired(false);

            builder.Property(u => u.PasswordHash)
                   .HasMaxLength(84)
                   .IsFixedLength(true)
                   .IsRequired(false);

            builder.Property(u => u.ConcurrencyStamp)
                   .IsConcurrencyToken()
                   .HasMaxLength(36)
                   .IsFixedLength(true)
                   .IsRequired(false);

            builder.HasOne(u => u.Person)
                   .WithOne()
                   .HasForeignKey<Person>(u => u.UserId);

            builder.HasIndex(u => u.UserName)
                   .IsUnique();

            builder.HasIndex(u => u.Email)
                   .IsUnique();
        }
    }
}
