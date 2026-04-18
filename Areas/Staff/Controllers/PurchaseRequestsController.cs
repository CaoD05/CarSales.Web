using CarSales.Web.Attributes;
using CarSales.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Areas.Staff.Controllers;

[Area("Staff")]
[RoleAuthorize("Staff")]
public class PurchaseRequestsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PurchaseRequestsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(string? status)
    {
        var requestsQuery = _context.PurchaseRequests
            .Include(x => x.Car)
            .Include(x => x.User)
            .Include(x => x.Staff)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            requestsQuery = requestsQuery.Where(x => x.Status == status);
        }

        ViewBag.Status = status;

        var requests = requestsQuery
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return View(requests);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStatus(int id, string status)
    {
        var request = _context.PurchaseRequests.Find(id);
        if (request == null)
        {
            return NotFound();
        }

        var staffIdRaw = HttpContext.Session.GetString("UserId");
        if (int.TryParse(staffIdRaw, out var staffId))
        {
            request.StaffId = staffId;
        }

        request.Status = status;
        request.UpdatedAt = DateTime.Now;

        _context.SaveChanges();

        TempData["Success"] = "Cập nhật trạng thái yêu cầu mua thành công.";
        return RedirectToAction(nameof(Index));
    }
}
