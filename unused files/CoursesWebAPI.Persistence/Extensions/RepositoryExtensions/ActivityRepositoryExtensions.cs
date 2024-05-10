using CoursesWebAPI.Core.Entities;
using System.Linq.Dynamic.Core;

namespace CoursesWebAPI.Persistence.Extensions.RepositoryExtensions
{
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
    }
}
