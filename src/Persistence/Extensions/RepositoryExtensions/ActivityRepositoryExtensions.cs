using CoursesWebAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CoursesWebAPI.Persistence.Extensions.RepositoryExtensions;

internal static class ActivityRepositoryExtensions
{
    public static IQueryable<Activity> Sort(this IQueryable<Activity> activitiesQuery, string? orderByQueryString)
    {
        var queryWithDefaultOrder = activitiesQuery.OrderBy(a => a.ActivityType)
                                                   .ThenBy(a => a.Begin);

        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return queryWithDefaultOrder;

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Activity>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return queryWithDefaultOrder;

        return activitiesQuery.OrderBy(orderQuery);
    }

    public static IQueryable<Activity> Search(this IQueryable<Activity> activities, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return activities;

        return activities.Where(c => EF.Functions.ILike(string.Join(' ', c.Teacher.FirsName, c.Teacher.LastName, c.Teacher.Patronomic), $"%{searchTerm}%"));
    }
}
