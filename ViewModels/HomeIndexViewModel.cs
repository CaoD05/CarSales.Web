using CarSales.Web.Models;

namespace CarSales.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Car> HeroCars { get; set; } = new();
        public List<CarType> CarTypes { get; set; } = new();
        public List<Car> Cars { get; set; } = new();
        public List<Car> DiscoverCars { get; set; } = new();
        public List<Car> FavoriteCars { get; set; } = new();
        public bool IsLoggedIn { get; set; }
        public int? SelectedCarTypeId { get; set; }
    }
}
