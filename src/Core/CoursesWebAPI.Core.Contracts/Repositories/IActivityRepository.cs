using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using System.Linq.Expressions;

namespace CoursesWebAPI.Core.Contracts.Repositories
{
    public interface IActivityRepository
    {
        public void Create(Activity activity);
        public Task<Activity?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<PageList<Activity>> GetByParametersAsync(ActivityQueryParameters queryParameters, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Activity>> GetByExpressionAsync(Expression<Func<Activity, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default);
        public void Update(Activity activity);
        public void Delete(Guid id);
    }
}
