using CoursesWebAPI.Core.Entities;

namespace CoursesWebAPI.Core.Contracts.Repositories
{
    public interface IActivityTypeRepository
    {
        public void Create(ActivityType activityType);
        public Task<bool> IsTypeExists(Guid id, CancellationToken cancellationToken = default);
        public Task<bool> IsTypeExists(string name, CancellationToken cancellationToken = default);
        public Task<ActivityType?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<ActivityType?> GetByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<IEnumerable<ActivityType>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);
        public void Update(ActivityType activityType);
        public void Delete(Guid id);
    }
}
