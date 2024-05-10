using CoursesWebAPI.Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace CoursesWebAPI.Persistence.Interceptors
{
    public sealed class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly ILogger logger;

        public AuditInterceptor(ILogger logger)
        {
            this.logger = logger;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if(eventData.Context is null)
            {
                return base.SavingChanges(eventData, result); ;
            }
            ModifyAuditableEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }
            ModifyAuditableEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ModifyAuditableEntities(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                                   .Where(entity => entity.GetType().Equals(typeof(AuditableEntity)) &&
                                   (entity.State is EntityState.Added ||
                                    entity.State is EntityState.Modified));

            var username = "Undifined"; //contextAccessor?.HttpContext?.User?.Identity?.Name ?? "Undefined";
            var time = DateTimeOffset.UtcNow;
            foreach (var entityEntry in entries)
            {
                var entity = (AuditableEntity)entityEntry.Entity;
                logger.Information("Entity {id} was modified at: {time} by: {username}", entity.Id, time, username);
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = time;
                        entity.CreatedBy = username;
                        break;

                    case EntityState.Modified:
                        entity.ModifiedAt = time;
                        entity.ModifiedBy = username;
                        break;

                    default:
                        continue;
                }
            }
        }

    }
}
