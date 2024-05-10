using CoursesWebAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Entities
{
    internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasMany(s => s.Activities)
                   .WithOne(c => c.Teacher)
                   .HasForeignKey(c => c.TeacherId);
        }
    }
}
