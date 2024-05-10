using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Entities;

internal sealed class PersonConfiguration : BaseEntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.ToTable("Persosns");

        builder.Property(p => p.FirsName)
               .HasMaxLength(30)
               .IsFixedLength(false)
               .IsRequired();

        builder.Property(p => p.LastName)
               .HasMaxLength(30)
               .IsFixedLength(false)
               .IsRequired();

        builder.Property(p => p.Patronomic)
               .HasMaxLength(30)
               .IsFixedLength(false)
               .IsRequired(false);

        builder.HasOne<User>()
               .WithOne(u => u.Person)
               .HasForeignKey<User>(u => u.PersonId);
    }
}
