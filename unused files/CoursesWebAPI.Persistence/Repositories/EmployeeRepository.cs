using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Persistence.Extensions.RepositoryExtensions;
using CoursesWebAPI.Persistence.Repositories.Base;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoursesWebAPI.Persistence.Repositories
{
    public sealed class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void Create(Employee employee) => Add(employee);

        public async Task<IEnumerable<Employee>> GetByExpressionAsync(Expression<Func<Employee, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default) => 
            await FindByCondition(expression, trackChanges).ToListAsync(cancellationToken);

        public async Task<Employee?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default) => 
            await FindByCondition(employee => employee.Id.Equals(id), trackChanges).FirstOrDefaultAsync(cancellationToken);

        public Task<PageList<Employee>> GetByParametersAsync(EmployeeQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var query = _context.Employees.Search(parameters.SearchTerm).Sort(parameters.OrderBy);
            return PageList<Employee>.ToPageListAsync(query, parameters.PageNumber, parameters.PageSize, cancellationToken);
        }

        public override void Update(Employee updatedEmployee) => base.Update(updatedEmployee);

        public void Delete(Guid id) => Delete(new Employee {Id = id });
    }
}
