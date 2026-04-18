using CarSales.Web.Attributes;
using CarSales.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Areas.Staff.Controllers;

[Area("Staff")]
[RoleAuthorize("Staff")]
public class DepositsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DepositsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(string? status)
    {
        var depositsQuery = _context.Deposits
            .Include(x => x.Car)
            .Include(x => x.User)
            .Include(x => x.Staff)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            depositsQuery = depositsQuery.Where(x => x.Status == status);
        }

        ViewBag.Status = status;

        var deposits = depositsQuery
            .OrderByDescending(x => x.DepositDate)
            .ToList();

        return View(deposits);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStatus(int id, string status)
    {
        var deposit = _context.Deposits.Find(id);
        if (deposit == null)
        {
            return NotFound();
        }

        var staffIdRaw = HttpContext.Session.GetString("UserId");
        if (int.TryParse(staffIdRaw, out var staffId))
        {
            deposit.StaffId = staffId;
        }

        deposit.Status = status;
        if (status == "Confirmed")
        {
            deposit.PaymentStatus = "Completed";
        }

        _context.SaveChanges();

        TempData["Success"] = "Cập nhật trạng thái đặt cọc thành công.";
        return RedirectToAction(nameof(Index));
    }
}
