using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.EntityServices
{
    public interface IActivityTypeService
    {
        public Task<ActivityTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<ActivityTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Result<ActivityTypeDto>> CreateAsync(ActivityTypeForCreatingDto activityTypeDto, CancellationToken cancellationToken = default);
        public Task<Result<ActivityTypeDto>> UpdateAsync(Guid id, ActivityTypeForUpdateDto activityTypeDto, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
