using CoursesWebAPI.Core.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoursesWebAPI.Persistence.Repositories.Base;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : Entity
{
    protected readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context) => _context = context;

    public virtual IQueryable<T> FindAll(bool trackChanges) =>
        trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();

    public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        trackChanges ? _context.Set<T>().Where(expression) : _context.Set<T>().Where(expression).AsNoTracking();

    public virtual void Add(T entity) => _context.Set<T>().Add(entity);

    public virtual void Delete(T entity) => _context.Set<T>().Remove(entity);

    public virtual void Update(T entity) => _context.Set<T>().Update(entity);
}
