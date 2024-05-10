using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CoursesWebAPI.Core.Common
{
    /// <summary>
    /// Represents <see cref="PageList{T}"/> based on <see cref="List{T}"/>.
    /// Stores a page of elements and parameters <see cref="PageListMetaData"/>. 
    /// </summary>
    /// <typeparam name="T">Type of stored elements.</typeparam>
    public sealed class PageList<T> : IEnumerable<T>
    {
        private readonly List<T> _items;

        /// <summary>
        /// Initialize <see cref="PageList{T}"> instance.
        /// </summary>
        /// <param name="items">Collection of elements for single page.</param>
        /// <param name="count">Total count of elements in collection.</param>
        /// <param name="pageNumber">Number of current page.</param>
        /// <param name="pageSize">Count of elements in one page.</param>
        /// <exception cref="ArgumentNullException">Throws when <param name="items"/> is null</exception>
        private PageList(IEnumerable<T> items, int count, int pageNumber, int pageSize) :
            this(items, new PageListMetaData(pageNumber, pageSize, count))
        { }

        /// <summary>
        /// Initialize <see cref="PageList{T}"> instance.
        /// </summary>
        /// <param name="items">Collection of elements for single page.</param>
        /// <param name="metaData">Contains parameters for the PageList.</param>
        /// <exception cref="ArgumentNullException">Throws is the <paramref name="metaData"/> is null</exception>
        private PageList(IEnumerable<T> items, PageListMetaData metaData)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(metaData, nameof(metaData));

            MetaData = metaData;
            _items = new(items);
        }
        
        public PageListMetaData MetaData { get; private set; }

        public List<T> Items => _items;

        /// <summary>
        /// Creates <see cref="PageList{T}"/> based on the source <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source">Collection of elements for single page.</param>
        /// <param name="count">Total count of elements in collection.</param>
        /// <param name="pageNumber">Number of current page.</param>
        /// <param name="pageSize">Count of elements in one page.</param>
        /// <returns><see cref="PageList{T}"/> based on the source collection.</returns>
        public static PageList<T> ToPageList(IEnumerable<T> source, int count, int pageNumber, int pageSize)
        {
            return new PageList<T>(source, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Creates <see cref="PageList{T}"/> based on the source <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source">Collection of elements for single page.</param>
        /// <param name="metaData">Contains parameters for the <see cref="PageList{T}"/>.</param>
        /// <returns><see cref="PageList{T}"/> based on the source collection.</returns>
        public static PageList<T> ToPageList(IEnumerable<T> source, PageListMetaData metaData) =>
            new(source, metaData);

        /// <summary>
        /// Create <see cref="PageList{T}"/> based on the <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <param name="query"><see cref="IQueryable{T}"/> query of elements.</param>
        /// <param name="pageNumber">Number of current page.</param>
        /// <param name="pageSize">Count of elements in one page.</param>
        /// <returns><see cref="PageList{T}"/> based on the source collection.</returns>
        public static async Task<PageList<T>> ToPageListAsync(IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            var totalCount = Queryable.Count(query);
            //var totalCount = await query.CountAsync(cancellationToken);

            if (pageNumber < 1 || pageSize < 1)
            {
                return new PageList<T>(Enumerable.Empty<T>(), totalCount, 0, 0);
            }
            //var items = await query.PageResult(pageNumber, pageSize).Queryable.ToListAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);
            return new PageList<T>(items, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Create <see cref="PageList{T}"/> based on the <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <param name="query"><see cref="IQueryable{T}"/> query of elements.</param>
        /// <param name="metaData"><see cref="PageListMetaData"/> parameters to query. </param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="PageList{T}"/> based on the source query.</returns>
        public static async Task<PageList<T>> ToPageListAsync(IQueryable<T> query, PageListMetaData metaData, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            ArgumentNullException.ThrowIfNull(metaData, nameof(metaData));

            if (metaData.PageSize == 0 || metaData.CurrentPage == 0 || metaData.TotalCount == 0)
                return PageList<T>.Empty();

            return await ToPageListAsync(query, metaData.CurrentPage, metaData.PageSize, cancellationToken);
        }

        /// <summary>
        /// Returns empty <see cref="PageList{T}"/>.
        /// </summary>
        /// <returns>Retunt empty <see cref="PageList{T}"/>.</returns>
        public static PageList<T> Empty() =>
            new(Enumerable.Empty<T>(), count: 0, pageNumber: 0, pageSize: 0);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
