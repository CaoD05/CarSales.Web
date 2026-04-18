using CarSales.Web.Attributes;
using CarSales.Web.Data;
using CarSales.Web.Helpers;
using CarSales.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorize("Admin")]
    public class CarsManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CarsManagementController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarType)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(cars);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car model, IFormFile? upload)
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
                return View(model);

            if (upload != null)
            {
                model.Thumbnail = await FileHelper.SaveImageAsync(upload, _environment.WebRootPath);
            }

            model.CreatedAt = DateTime.Now;
            model.Status = string.IsNullOrWhiteSpace(model.Status) ? "Available" : model.Status;

            _context.Cars.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm xe thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null)
                return NotFound();

            LoadDropdowns();
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Car model, IFormFile? upload)
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
                return View(model);

            var car = _context.Cars.Find(model.CarId);
            if (car == null)
                return NotFound();

            car.CarName = model.CarName;
            car.BrandId = model.BrandId;
            car.CarTypeId = model.CarTypeId;
            car.Price = model.Price;
            car.Year = model.Year;
            car.Mileage = model.Mileage;
            car.FuelType = model.FuelType;
            car.Transmission = model.Transmission;
            car.Seats = model.Seats;
            car.Color = model.Color;
            car.Engine = model.Engine;
            car.Origin = model.Origin;
            car.Description = model.Description;
            car.Status = model.Status;
            car.IsFeatured = model.IsFeatured;
            car.UpdatedAt = DateTime.Now;

            if (upload != null)
            {
                car.Thumbnail = await FileHelper.SaveImageAsync(upload, _environment.WebRootPath);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật xe thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var car = _context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarType)
                .FirstOrDefault(x => x.CarId == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null)
                return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa xe thành công";
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            ViewBag.BrandId = new SelectList(_context.Brands.Where(x => x.IsActive).ToList(), "BrandId", "BrandName");
            ViewBag.CarTypeId = new SelectList(_context.CarTypes.ToList(), "CarTypeId", "TypeName");
        }
    }
}