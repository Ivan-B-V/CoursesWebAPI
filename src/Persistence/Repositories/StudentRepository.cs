using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Persistence.Extensions;
using CoursesWebAPI.Persistence.Extensions.RepositoryExtensions;
using CoursesWebAPI.Persistence.Repositories.Base;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoursesWebAPI.Persistence.Repositories;

public sealed class StudentRepository : RepositoryBase<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context) { }

    public void Create(Student student) => Add(student);

    public void Delete(Guid id) => Delete(new Student { Id = id });

    public async Task<IEnumerable<Student>> GetByExpressionAsync(Expression<Func<Student, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(expression, trackChanges).ToListAsync(cancellationToken);
    }

    public async Task<Student?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(s => s.Id.Equals(id), trackChanges)
                    .Include(s => s.Contracts)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<PageList<Student>> GetByParametersAsync(PersonQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        var strudentsQuery = GetQueryByParameters(parameters, false);
        return PageList<Student>.ToPageListAsync(strudentsQuery, parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<PageList<Student>> GetStudentsAsync(PersonQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = GetQueryByParameters(parameters, false);
        return await PageList<Student>.ToPageListAsync(query, parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public override void Update(Student updatedStudent) => base.Update(updatedStudent);

    private IQueryable<Student> GetQueryByParameters(PersonQueryParameters parameters, bool trackChanges) =>
        FindAll(trackChanges)
       .Search(parameters.SearchTerm)
       .Sort(parameters.OrderBy)
       .TagWith($"Students with searchTerm: {parameters.SearchTerm}, ordered by: {parameters.OrderBy}");
}
