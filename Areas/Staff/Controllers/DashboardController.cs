using CarSales.Web.Attributes;
using CarSales.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    [RoleAuthorize("Staff")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var staffIdRaw = HttpContext.Session.GetString("UserId");
            int.TryParse(staffIdRaw, out var staffId);

            ViewBag.NewRequests = _context.PurchaseRequests.Count(x => x.Status == "New");
            ViewBag.MyProcessingRequests = _context.PurchaseRequests.Count(x => x.StaffId == staffId && x.Status == "Processing");
            ViewBag.WaitingDeposits = _context.Deposits.Count(x => x.Status == "WaitingConfirm" || x.Status == "Pending");
            ViewBag.MyHandledDeposits = _context.Deposits.Count(x => x.StaffId == staffId && x.Status == "Confirmed");
            ViewBag.LatestRequests = _context.PurchaseRequests
                .Include(x => x.Car)
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Take(6)
                .ToList();
            ViewBag.LatestDeposits = _context.Deposits
                .Include(x => x.Car)
                .Include(x => x.User)
                .OrderByDescending(x => x.DepositDate)
                .Take(6)
                .ToList();

            return View();
        }
    }
}