namespace CarSales.Web.ViewModels
{
    public class CarFilterViewModel
    {
        public string? Keyword { get; set; }
        public int? BrandId { get; set; }
        public int? CarTypeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? FuelType { get; set; }
        public string? Transmission { get; set; }
        public int Page { get; set; } = 1;
    }
}