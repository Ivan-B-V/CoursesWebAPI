using CoursesWebAPI.Core.Entities;
using System.Linq.Dynamic.Core;

namespace CoursesWebAPI.Persistence.Extensions
{
    internal static class StudentRepositoryExtensions
    {
        public static IQueryable<Student> Search(this IQueryable<Student> students, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return students;
            return students.Where(s => s.FirsName.Contains(searchTerm) ||
                                       s.LastName.Contains(searchTerm));
        }

        public static IQueryable<Student> Sort(this IQueryable<Student> students, string? orderByQueryString)
        {
            var queryWithDefaultOrder = students.OrderBy(s => s.LastName)
                                                .ThenBy(s => s.FirsName)
                                                .ThenBy(s => s.Patronomic);

            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryWithDefaultOrder;

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Student>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return queryWithDefaultOrder;

            return students.OrderBy(orderQuery);
        }
    }
}
