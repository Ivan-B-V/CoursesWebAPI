using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.EntityServices
{
    public interface IEmployeeService
    {
        public Task<PageList<EmployeeDto>> GetEmployeesAsync(PersonQueryParameters queryParameters, CancellationToken cancellationToken = default);
        public Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<EmployeeDto>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        public Task<Result<EmployeeDto>> CreateAsync(EmployeeForCreatingDto employeeDto, CancellationToken cancellationToken = default);
        public Task<Result<EmployeeDto>> UpdateAsync(Guid id,EmployeeForUpdateDto employeeDto, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
