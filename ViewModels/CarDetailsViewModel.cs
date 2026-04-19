using CarSales.Web.Models;

namespace CarSales.Web.ViewModels
{
    public class CarDetailsViewModel
    {
        public Car Car { get; set; } = null!;
        public List<string> GalleryImages { get; set; } = new();
        public List<Car> SimilarCars { get; set; } = new();
        public bool IsFavorited { get; set; }
        public bool IsLoggedIn { get; set; }
    public string CurrentUserRole { get; set; } = string.Empty;
    }
}
