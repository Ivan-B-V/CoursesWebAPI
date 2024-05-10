using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesWebAPI.Persistence.Configurations.Identity
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(x => x.Id);

            var permissions = Enum.GetValues<Permissions>()
                                  .Select(p => new Permission 
                                  {
                                      Id = (int)p,
                                      Name = p.ToString()
                                  });

            builder.HasData(permissions);
        }
    }
}
