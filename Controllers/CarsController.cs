using CarSales.Web.Data;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? carTypeId, int? brandId, string? keyword, int page = 1, int pageSize = 9)
        {
            keyword = keyword?.Trim();
            page = page < 1 ? 1 : page;
            pageSize = pageSize <= 0 ? 9 : pageSize;

            ViewBag.CarTypes = _context.CarTypes
                .OrderBy(x => x.TypeName)
                .ToList();
            ViewBag.Brands = _context.Brands
                .Where(x => x.IsActive)
                .OrderBy(x => x.BrandName)
                .ToList();
            ViewBag.SelectedCarTypeId = carTypeId;
            ViewBag.SelectedBrandId = brandId;
            ViewBag.Keyword = keyword;

            var carsQuery = _context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarType)
                .AsQueryable();

            if (carTypeId.HasValue)
            {
                carsQuery = carsQuery.Where(x => x.CarTypeId == carTypeId.Value);
                ViewBag.SelectedCarTypeName = _context.CarTypes
                    .Where(x => x.CarTypeId == carTypeId.Value)
                    .Select(x => x.TypeName)
                    .FirstOrDefault();
            }

            if (brandId.HasValue)
            {
                carsQuery = carsQuery.Where(x => x.BrandId == brandId.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                carsQuery = carsQuery.Where(x =>
                    x.CarName.Contains(keyword) ||
                    (x.Brand != null && x.Brand.BrandName.Contains(keyword)) ||
                    (x.CarType != null && x.CarType.TypeName.Contains(keyword)));
            }

            var totalItems = carsQuery.Count();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
            page = Math.Min(page, totalPages);

            var cars = carsQuery
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;

            return View(cars);
        }

        public IActionResult Details(int id)
        {
            int.TryParse(HttpContext.Session.GetString("UserId"), out var userId);
            var isLoggedIn = userId > 0;

            var car = _context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarType)
                .Include(x => x.CarImages)
                .FirstOrDefault(x => x.CarId == id);

            if (car == null)
            {
                return NotFound();
            }

            var galleryImages = car.CarImages?
                .Where(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
                .OrderByDescending(x => x.IsPrimary)
                .ThenByDescending(x => x.UploadedAt)
                .Select(x => x.ImageUrl)
                .Distinct()
                .ToList() ?? new List<string>();

            if (!string.IsNullOrWhiteSpace(car.Thumbnail) && !galleryImages.Contains(car.Thumbnail))
            {
                galleryImages.Insert(0, car.Thumbnail);
            }

            if (!galleryImages.Any())
            {
                galleryImages.Add("/uploads/img/car.png");
            }

            var similarCars = _context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarType)
                .Where(x => x.CarId != id && x.CarTypeId == car.CarTypeId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(4)
                .ToList();

            var vm = new CarDetailsViewModel
            {
                Car = car,
                GalleryImages = galleryImages,
                SimilarCars = similarCars,
                IsLoggedIn = isLoggedIn,
                IsFavorited = isLoggedIn && _context.Favorites.Any(x => x.UserId == userId && x.CarId == id)
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Buy(int id)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var car = _context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarType)
                .FirstOrDefault(x => x.CarId == id);

            if (car == null)
            {
                return NotFound();
            }

            var vm = new CarBuyPageViewModel
            {
                Car = car,
                SuggestedDepositAmount = GetSuggestedDepositAmount(car.Price),
                HasOpenPurchaseRequest = _context.PurchaseRequests.Any(x =>
                    x.UserId == user.UserId &&
                    x.CarId == id &&
                    (x.Status == "New" || x.Status == "Processing")),
                HasOpenDeposit = _context.Deposits.Any(x =>
                    x.UserId == user.UserId &&
                    x.CarId == id &&
                    (x.Status == "WaitingConfirm" || x.Status == "Pending"))
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult MyTickets()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var vm = new UserTicketsViewModel
            {
                PurchaseRequests = _context.PurchaseRequests
                    .Include(x => x.Car)
                    .Where(x => x.UserId == user.UserId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList(),
                Deposits = _context.Deposits
                    .Include(x => x.Car)
                    .Where(x => x.UserId == user.UserId)
                    .OrderByDescending(x => x.DepositDate)
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleFavorite(int id, string? returnUrl)
        {
            int.TryParse(HttpContext.Session.GetString("UserId"), out var userId);
            if (userId <= 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingFavorite = _context.Favorites.FirstOrDefault(x => x.UserId == userId && x.CarId == id);
            if (existingFavorite == null)
            {
                _context.Favorites.Add(new Models.Favorite
                {
                    UserId = userId,
                    CarId = id,
                    CreatedAt = DateTime.Now
                });
                TempData["Success"] = "Đã thêm xe vào danh sách yêu thích";
            }
            else
            {
                _context.Favorites.Remove(existingFavorite);
                TempData["Success"] = "Đã xóa xe khỏi danh sách yêu thích";
            }

            _context.SaveChanges();

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDepositTicket(int id, string? note, string? returnUrl)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var car = _context.Cars.FirstOrDefault(x => x.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            var hasPendingDeposit = _context.Deposits.Any(x =>
                x.UserId == user.UserId &&
                x.CarId == id &&
                (x.Status == "WaitingConfirm" || x.Status == "Pending"));

            if (!hasPendingDeposit)
            {
                var depositAmount = GetSuggestedDepositAmount(car.Price);

                _context.Deposits.Add(new Models.Deposit
                {
                    UserId = user.UserId,
                    CarId = id,
                    DepositAmount = depositAmount,
                    PaymentMethod = "Online",
                    PaymentStatus = "Pending",
                    Status = "WaitingConfirm",
                    DepositDate = DateTime.Now,
                    Note = string.IsNullOrWhiteSpace(note) ? "Yêu cầu đặt cọc từ trang mua xe" : note.Trim()
                });

                _context.SaveChanges();
                TempData["Success"] = "Yêu cầu đặt cọc đã được gửi. UTC Car sẽ liên hệ bạn sớm.";
            }
            else
            {
                TempData["Success"] = "Bạn đã có yêu cầu đặt cọc chờ xác nhận cho xe này.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(MyTickets));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePurchaseRequestTicket(int id, string? message, string? returnUrl)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var car = _context.Cars.FirstOrDefault(x => x.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            var hasOpenRequest = _context.PurchaseRequests.Any(x =>
                x.UserId == user.UserId &&
                x.CarId == id &&
                (x.Status == "New" || x.Status == "Processing"));

            if (!hasOpenRequest)
            {
                _context.PurchaseRequests.Add(new Models.PurchaseRequest
                {
                    UserId = user.UserId,
                    CarId = id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = string.IsNullOrWhiteSpace(user.Phone) ? "0900000000" : user.Phone!,
                    Message = string.IsNullOrWhiteSpace(message) ? "Yêu cầu mua xe từ trang mua xe" : message.Trim(),
                    Status = "New",
                    CreatedAt = DateTime.Now
                });

                _context.SaveChanges();
                TempData["Success"] = "Yêu cầu mua xe đã được gửi thành công.";
            }
            else
            {
                TempData["Success"] = "Bạn đã có yêu cầu mua xe đang được xử lý cho xe này.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(MyTickets));
        }

        private Models.User? GetCurrentUser()
        {
            int.TryParse(HttpContext.Session.GetString("UserId"), out var userId);
            if (userId <= 0)
            {
                return null;
            }

            return _context.Users.FirstOrDefault(x => x.UserId == userId && x.IsActive);
        }

        private static decimal GetSuggestedDepositAmount(decimal carPrice)
        {
            var depositAmount = Math.Round(carPrice * 0.1m, 0, MidpointRounding.AwayFromZero);
            return depositAmount < 1000000m ? 1000000m : depositAmount;
        }
    }
}