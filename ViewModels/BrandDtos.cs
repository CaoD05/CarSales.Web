namespace CarSales.Web.ViewModels
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUpdateBrandDto
    {
        public string BrandName { get; set; } = string.Empty;
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}