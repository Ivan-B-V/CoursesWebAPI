using CoursesWebAPI.Core.Identity;
using System.Linq.Dynamic.Core;

namespace CoursesWebAPI.Persistence.Extensions
{
    public static class UserManagerExtensions
    {
        public static IQueryable<User> Search(this IQueryable<User> users, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return users;
            return users.Where(user => user.Email != null && user.Email.Contains(searchTerm) ||
                                       user.UserName != null && user.UserName.Contains(searchTerm));
        }

        public static IQueryable<User> Sort(this IQueryable<User> users, string? orderByQueryString)
        {
            var queryWithDefaultOrder = users.OrderBy(c => c.Email);

            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryWithDefaultOrder;

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return queryWithDefaultOrder;

            return users.OrderBy(orderQuery);
        }
    }
}
