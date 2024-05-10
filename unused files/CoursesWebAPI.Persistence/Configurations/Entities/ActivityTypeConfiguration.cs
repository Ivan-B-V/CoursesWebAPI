using CoursesWebAPI.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Entities
{
    internal sealed class ActivityTypeConfiguration : BaseEntityConfiguration<ActivityType>
    {
        public override void Configure(EntityTypeBuilder<ActivityType> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                   .HasMaxLength(20)
                   .IsRequired();
            
            builder.Property(x => x.Description)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.HasIndex(x => x.Name)
                   .IsUnique();
        }
    }
}
