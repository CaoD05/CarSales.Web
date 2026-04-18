using System.Diagnostics;
using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? carTypeId)
        {
            int.TryParse(HttpContext.Session.GetString("UserId"), out var userId);
            var isLoggedIn = userId > 0;

            var carTypes = _context.CarTypes
                .OrderBy(x => x.TypeName)
                .ToList();

            var selectedCarTypeId = carTypeId ?? carTypes.FirstOrDefault()?.CarTypeId;

            var carsQuery = _context.Cars
                .Include(x => x.CarType)
                .Where(x => x.Status == "Available")
                .AsQueryable();

            if (selectedCarTypeId.HasValue)
            {
                carsQuery = carsQuery.Where(x => x.CarTypeId == selectedCarTypeId.Value);
            }

            var cars = carsQuery
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.CreatedAt)
                .ToList();

            var discoverCars = _context.Cars
                .Include(x => x.CarType)
                .Where(x => x.Status == "Available")
                .OrderByDescending(x => x.CreatedAt)
                .Take(8)
                .AsEnumerable()
                .OrderBy(_ => Random.Shared.Next())
                .Take(3)
                .ToList();

            var heroCars = _context.Cars
                .Include(x => x.Brand)
                .Where(x => x.Status == "Available")
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.CreatedAt)
                .ToList();

            var favoriteCars = isLoggedIn
                ? _context.Favorites
                    .Where(x => x.UserId == userId && x.Car != null)
                    .Include(x => x.Car!)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Car!)
                        .ThenInclude(x => x.CarType)
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => x.Car!)
                    .Take(4)
                    .ToList()
                : new List<Car>();

            var model = new HomeIndexViewModel
            {
                HeroCars = heroCars,
                CarTypes = carTypes,
                Cars = cars,
                DiscoverCars = discoverCars,
                FavoriteCars = favoriteCars,
                IsLoggedIn = isLoggedIn,
                SelectedCarTypeId = selectedCarTypeId
            };

            return View(model);
        }

        public IActionResult GetCarsByType(int? carTypeId)
        {
            var carsQuery = _context.Cars
                .Include(x => x.CarType)
                .Where(x => x.Status == "Available")
                .AsQueryable();

            if (carTypeId.HasValue)
            {
                carsQuery = carsQuery.Where(x => x.CarTypeId == carTypeId.Value);
            }

            var cars = carsQuery
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.CreatedAt)
                .ToList();

            return PartialView("_CarExploreGrid", cars);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
