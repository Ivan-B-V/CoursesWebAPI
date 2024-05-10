using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Persistence.Configurations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations
{
    internal sealed class ContractConfiguration : BaseEntityConfiguration<Contract>
    {
        public override void Configure(EntityTypeBuilder<Contract> builder)
        {
            base.Configure(builder);

            builder.ToTable("Contracts");

            builder.Property(c => c.Number)
                   .HasMaxLength(32)
                   .IsRequired();

            builder.Property(c => c.Cost)
                   .HasPrecision(7, 2)
                   .HasDefaultValue(0m);
            
            builder.Property(c => c.Closed)
                   .HasDefaultValue(false);
            
            builder.HasOne(c => c.Student)
                   .WithMany(s => s.Contracts)
                   .HasForeignKey(c => c.StudentId)
                   .IsRequired();

            builder.HasMany(x => x.Activities)
                   .WithMany(x => x.Contracts);

            builder.HasIndex(x => x.Number)
                   .IsUnique();
        }
    }
}