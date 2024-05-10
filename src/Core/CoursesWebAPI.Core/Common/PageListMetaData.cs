namespace CoursesWebAPI.Core.Common
{
    public record PageListMetaData
    {
        public int CurrentPage { get; init; }
        public int TotalPages { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PageListMetaData(int currentPage, int pageSize, int totalCount)
        {
            if (totalCount < 0 )
                throw new ArgumentOutOfRangeException(nameof(totalCount));

            if (currentPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(currentPage));

            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            CurrentPage = currentPage;
            TotalCount = totalCount;
            PageSize = pageSize;
            if (pageSize <= 0 || totalCount <= 0)
            {
                TotalPages = 0;
            }
            else 
            {
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            }
        }
    }
}
