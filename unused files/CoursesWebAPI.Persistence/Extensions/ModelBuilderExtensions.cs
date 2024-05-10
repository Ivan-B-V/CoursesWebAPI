using CoursesWebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebAPI.Persistence.Extensions
{
    internal static class ModelBuilderExtensions
    {
        public static void SeedRoles(this ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role 
                {
                    Id = new Guid("dc390911-2b01-42c5-a396-604289a8c08b"),
                    Name = "HolyFather",
                    NormalizedName = "HOLYFATHER"
                },
                new Role
                {
                    Id = new Guid("43d804cc-c721-4bd9-bb27-f9903b83d0bb"),
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },
                new Role
                {
                    Id = new Guid("cf3c9527-51dd-4b9e-8a34-e00d7c5b341e"),
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new Role
                {
                    Id = new Guid("84a2e635-0b80-4e30-af39-7a57df06ba3d"),
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                },
                new Role
                {
                    Id = new Guid("f4b26c13-a1b7-43db-831d-277ca58d3ff6"),
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new Role
                {
                    Id = new Guid("02e9db0c-9e8b-4aa1-a0a4-db85b8a55dc4"),
                    Name = "HR",
                    NormalizedName = "HR"
                });
        }

        public static void SeedUsers(this ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User 
                {
                    Id = new Guid("f31733be-7d8c-422d-81c5-9feb708226e2"),
                    UserName = "Ivan",
                    NormalizedUserName = "IVAN",
                    Email = "hammer000destroyer@gmail.com",
                    NormalizedEmail = "HAMMER000DESTROYER@GMAIL.COM",
                    EmailConfirmed = true,
                    PhoneNumber = ""
                });

            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> 
                {
                    UserId = new Guid("f31733be-7d8c-422d-81c5-9feb708226e2"),
                    RoleId = new Guid("dc390911-2b01-42c5-a396-604289a8c08b")
                });
        }
    }
}
