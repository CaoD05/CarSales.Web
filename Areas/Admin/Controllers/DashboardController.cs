using CarSales.Web.Attributes;
using CarSales.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleAuthorize("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalCars = _context.Cars.Count();
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalStaff = _context.Users.Count(x => x.Role != null && x.Role.RoleName == "Staff");
            ViewBag.PendingRequests = _context.PurchaseRequests.Count(x => x.Status == "New");
            ViewBag.PendingDeposits = _context.Deposits.Count(x => x.Status == "Pending" || x.Status == "WaitingConfirm");
            ViewBag.RecentCars = _context.Cars
                .Include(x => x.Brand)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToList();
            ViewBag.RecentUsers = _context.Users
                .Include(x => x.Role)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToList();

            return View();
        }
    }
}