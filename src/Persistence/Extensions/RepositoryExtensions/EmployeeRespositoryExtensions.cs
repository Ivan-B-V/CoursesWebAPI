using CoursesWebAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CoursesWebAPI.Persistence.Extensions.RepositoryExtensions;

public static class EmployeeRespositoryExtensions
{
    public static IQueryable<Employee> Search(this IQueryable<Employee> query, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return query;
        }
        return query.Where(e => EF.Functions.ILike(string.Join(' ', e.FirsName, e.LastName, e.Patronomic), $"%{searchTerm}%"));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string? orderByQueryString)
    {
        var queryWithDefaultOrder = employees.OrderBy(e => e.LastName)
                                             .ThenBy(e => e.FirsName)
                                             .ThenBy(e => e.Patronomic);

        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return queryWithDefaultOrder;

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Contract>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return queryWithDefaultOrder;

        return employees.OrderBy(orderQuery);
    }
}
