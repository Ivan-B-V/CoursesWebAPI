using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebAPI.Persistence.Repositories
{
    public sealed class ActivityTypeRepository : RepositoryBase<ActivityType>, IActivityTypeRepository
    {
        public ActivityTypeRepository(ApplicationDbContext context) : base(context)
        { }

        public void Create(ActivityType activityType) => Add(activityType);


        public async Task<IEnumerable<ActivityType>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default)
        {
            return await FindAll(trackChanges).ToListAsync(cancellationToken);
        }

        public async Task<ActivityType?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default)
        {
            return await FindByCondition(type => type.Id.Equals(id), trackChanges).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ActivityType?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken = default)
        {
            return await FindByCondition(type => type.Name.Equals(name), trackChanges).FirstOrDefaultAsync(cancellationToken);
        }

        public override void Update(ActivityType activityType) => base.Update(activityType);

        public void Delete(Guid id) => Delete(id);

        public async Task<bool> IsTypeExists(Guid id, CancellationToken cancellationToken = default) =>
            await FindByCondition(t => t.Id.Equals(id), trackChanges: false).AnyAsync(cancellationToken);

        public async Task<bool> IsTypeExists(string name, CancellationToken cancellationToken = default) => 
            await FindByCondition(t => t.Name.Equals(name), trackChanges: false).AnyAsync(cancellationToken);
    }
}
