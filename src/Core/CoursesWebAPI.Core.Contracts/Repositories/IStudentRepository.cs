using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using System.Linq.Expressions;

namespace CoursesWebAPI.Core.Contracts.Repositories
{
    public interface IStudentRepository
    {
        public void Create(Student student);
        public Task<PageList<Student>> GetByParametersAsync(PersonQueryParameters parameters, CancellationToken cancellationToken = default);
        public Task<Student?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Student>> GetByExpressionAsync(Expression<Func<Student, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default);
        public void Update(Student updatedStudent);
        public void Delete(Guid id);
    }
}
