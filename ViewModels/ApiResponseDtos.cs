namespace CarSales.Web.ViewModels
{
    // Generic API Response
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    // Paginated Response
    public class PaginatedResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PaginationInfo Pagination { get; set; } = new();
        public FilterInfo? Filters { get; set; }
        public List<T> Data { get; set; } = [];
    }

    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class FilterInfo
    {
        public string? Keyword { get; set; }
        public int? BrandId { get; set; }
        public int? CarTypeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}