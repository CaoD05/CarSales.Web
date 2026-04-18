using CarSales.Web.Attributes;
using CarSales.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Areas.Admin.Controllers;

[Area("Admin")]
[RoleAuthorize("Admin")]
public class UsersManagementController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsersManagementController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(string? keyword, string? role)
    {
        keyword = keyword?.Trim();

        var usersQuery = _context.Users
            .Include(x => x.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            usersQuery = usersQuery.Where(x => x.FullName.Contains(keyword) || x.Email.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            usersQuery = usersQuery.Where(x => x.Role != null && x.Role.RoleName == role);
        }

        ViewBag.Keyword = keyword;
        ViewBag.Role = role;

        var users = usersQuery
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleStatus(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = !user.IsActive;
        _context.SaveChanges();

        TempData["Success"] = "Cập nhật trạng thái tài khoản thành công.";
        return RedirectToAction(nameof(Index));
    }
}
