namespace MyPersonalLibrary.Server.Models.Utils
{
    public class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        private PaginatedResult(List<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items.AsReadOnly();
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        }

        public static PaginatedResult<T> Paginate(List<T> items, int totalItems, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));
            return new PaginatedResult<T>(items, totalItems, pageNumber, pageSize);
        }
    }

}
