using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using System.Linq.Expressions;

namespace CoursesWebAPI.Core.Contracts.Repositories
{
    public interface IEmployeeRepository
    {
        public void Create(Employee employee);
        public Task<PageList<Employee>> GetByParametersAsync(PersonQueryParameters parameters, CancellationToken cancellationToken = default);
        public Task<Employee?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Employee>> GetByExpressionAsync(Expression<Func<Employee, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default);
        public void Update(Employee updatedEmployee);
        public void Delete(Guid id);
    }
}
