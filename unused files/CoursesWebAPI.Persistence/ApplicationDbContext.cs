using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Persistence.Configurations;
using CoursesWebAPI.Persistence.Configurations.Entities;
using CoursesWebAPI.Persistence.Configurations.Identity;
using CoursesWebAPI.Persistence.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CoursesWebAPI.Persistence
{
    public sealed class ApplicationDbContext : IdentityDbContext<
        User,
        Role,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        private readonly IHttpContextAccessor contextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            contextAccessor = this.GetService<IHttpContextAccessor>();
        }

        public DbSet<RefreshToken> RefreshTokens { get; private set; }

        public DbSet<Contract> Contracts { get; private set; }

        public DbSet<Activity> Activities { get; private set; }

        public DbSet<ActivityType> ActivityTypes { get; private set; }

        public DbSet<Student> Students { get; private set; }

        public DbSet<Employee> Employees { get; private set; }

        public DbSet<Permission> Permissions { get; private set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // identity configuration
            builder.ApplyConfiguration(new PermissionConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new IdentityUserTokenConfiguration());
            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());

            // data configuration
            builder.ApplyConfiguration(new PersonConfiguration());
            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new StudentConfiguration());
            builder.ApplyConfiguration(new ContractConfiguration());
            builder.ApplyConfiguration(new ActivityConfiguration());
            builder.ApplyConfiguration(new ActivityTypeConfiguration());

            //seed data
            builder.SeedRoles();
            builder.SeedUsers();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //UpdateAuditableEntitiesBeforeSave();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            //UpdateAuditableEntitiesBeforeSave();
            return base.SaveChanges();
        }

        private void UpdateAuditableEntitiesBeforeSave()
        {
            var entries = ChangeTracker.Entries()
                                       .Where(entity => entity.GetType().Equals(typeof(AuditableEntity)) &&
                                       (entity.State is EntityState.Added ||
                                        entity.State is EntityState.Modified));

            var username = contextAccessor?.HttpContext?.User?.Identity?.Name ?? "Undefined";
            foreach (var entityEntry in entries)
            {
                var entity = (AuditableEntity)entityEntry.Entity;
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTimeOffset.Now;
                        entity.CreatedBy = username;
                        break;

                    case EntityState.Modified:
                        entity.ModifiedAt = DateTimeOffset.Now;
                        entity.ModifiedBy = username;
                        break;

                    default:
                        continue;
                }
            }
        }
    }
}
