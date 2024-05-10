using CoursesWebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Fingerprint)
                   .HasMaxLength(200)
                   .HasDefaultValue(null)
                   .IsRequired();
            
            builder.Property(t => t.UserId)
                   .IsRequired();

            builder.Property(t => t.JwtId)
                   .IsRequired();
            
            builder.Property(t => t.Created)
                   .IsRequired();
            
            builder.Property(t => t.Expires)
                   .IsRequired();
        }
    }
}
