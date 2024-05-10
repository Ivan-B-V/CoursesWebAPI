using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.EntityServices
{
    public interface IActivityService
    {
        public Task<Result<ActivityFullDto>> CreateAsync(ActivityForCreatingDto activityDto, CancellationToken cancellationToken = default);
        public Task<Result<Guid>> UpdateAsync(Guid id, ActivityForUpdateDto activityDto, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<ActivityFullDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<ActivityFullDto>> GetByStudentIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<PageList<ActivityFullDto>> GetByParametersAsync(ActivityQueryParameters queryParameters, CancellationToken cancellationToken = default);
    }
}
