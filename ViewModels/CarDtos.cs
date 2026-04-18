namespace CarSales.Web.ViewModels
{
    // DTO cho response
    public class CarDto
    {
        public int CarId { get; set; }
        public string CarName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int? Mileage { get; set; }
        public string? FuelType { get; set; }
        public string? Transmission { get; set; }
        public int? Seats { get; set; }
        public string? Color { get; set; }
        public string? Engine { get; set; }
        public string? Origin { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public int CarTypeId { get; set; }
        public string? CarTypeName { get; set; }
    }

    // DTO cho create/update request
    public class CreateUpdateCarDto
    {
        public string CarName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public int CarTypeId { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public int? Mileage { get; set; }
        public string? FuelType { get; set; }
        public string? Transmission { get; set; }
        public int? Seats { get; set; }
        public string? Color { get; set; }
        public string? Engine { get; set; }
        public string? Origin { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public string Status { get; set; } = "Available";
        public bool IsFeatured { get; set; }
    }
}