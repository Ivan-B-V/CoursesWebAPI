using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Persistence.Extensions.RepositoryExtensions;
using CoursesWebAPI.Persistence.Repositories.Base;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebAPI.Persistence.Repositories
{
    public sealed class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context) { }

        public void Create(Activity activity) => Add(activity);

        public async Task<Activity?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default)
        {
            return await FindByCondition(a => a.Id.Equals(id), trackChanges)
                        .Include(a => a.ActivityType)
                        .Include(a => a.Teacher)
                        .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PageList<Activity>> GetByParametersAsync(ActivityQueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            IQueryable<Activity> activitiesQuery = _context.Activities.AsNoTracking();
            if (queryParameters.From != default && queryParameters.To != default)
            {
                activitiesQuery = activitiesQuery.Where(a => (a.Begin >= queryParameters.From && a.Begin <= queryParameters.To) ||
                                                             (a.End >= queryParameters.From && a.End <= queryParameters.To))
                                                 .TagWith($"Activities From: {queryParameters.From}, To: {queryParameters.To}."); ;
            }
            //activitiesQuery = activitiesQuery.Include(a => a.ActivityType)
            //                                 .Include(a => a.Teacher)
            //                                 .AsSplitQuery();

            activitiesQuery = activitiesQuery.Sort(queryParameters.OrderBy)
                                             .TagWith($"Activities ordered by: {queryParameters.OrderBy}");

            return await PageList<Activity>.ToPageListAsync(activitiesQuery, queryParameters.PageNumber, queryParameters.PageSize, cancellationToken);
        }

        public override void Update(Activity activity) => base.Update(activity);

        public void Delete(Guid id) => Delete(new Activity { Id = id });
    }
}
