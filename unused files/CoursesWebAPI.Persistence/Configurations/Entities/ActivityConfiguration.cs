using CoursesWebAPI.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Entities
{
    internal sealed class ActivityConfiguration : BaseEntityConfiguration<Activity>
    {
        public override void Configure(EntityTypeBuilder<Activity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Description)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(x => x.Begin)
                   .IsRequired();

            builder.Property(x => x.End)
                   .IsRequired();

            builder.HasOne(x => x.ActivityType)
                   .WithMany()
                   .HasForeignKey(x => x.ActivityTypeId);

            builder.HasOne(x => x.Teacher)
                   .WithMany()
                   .HasForeignKey(x => x.TeacherId);

            builder.HasMany(x => x.Contracts)
                   .WithMany(x => x.Activities);
        }
    }
}
