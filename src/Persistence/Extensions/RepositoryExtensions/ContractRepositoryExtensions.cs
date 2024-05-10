using CoursesWebAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CoursesWebAPI.Persistence.Extensions.RepositoryExtensions;

public static class ContractRepositoryExtensions
{
    public static IQueryable<Contract> Search(this IQueryable<Contract> contracts, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return contracts;
        return contracts.Where(c => EF.Functions.Like(c.Number, $"%{searchTerm}%") || 
                                    (c.Student != null &&
                                    EF.Functions.Like(string.Join(' ', c.Student.FirsName, c.Student.LastName, c.Student.Patronomic), $"%{searchTerm}%")));
    }

    public static IQueryable<Contract> Sort(this IQueryable<Contract> contracts, string? orderByQueryString)
    {
        var queryWithDefaultOrder = contracts.OrderBy(c => c.Number);

        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return queryWithDefaultOrder;

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Contract>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return queryWithDefaultOrder;

        return contracts.OrderBy(orderQuery);
    }
}
