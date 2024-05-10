using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;

namespace CoursesWebAPI.Application.Contracts.EntityServices
{
    public interface IStudentService
    {
        public Task<PageList<StudentForUpdateDto>> GetStudentsAsync(PersonQueryParameters queryParameters, CancellationToken cancellationToken = default);
        public Task<StudentForUpdateDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<StudentDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<StudentForUpdateDto>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        public Task<StudentDto> CreateAsync(StudentForCreatingDto studentDto, CancellationToken cancellationToken = default);
        public Task<StudentDto> UpdateAsync(StudentForUpdateDto studentDto, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
