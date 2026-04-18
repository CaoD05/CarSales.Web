using CarSales.Web.Data;
using CarSales.Web.Helpers;
using CarSales.Web.Models;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string hashedPassword = PasswordHelper.HashPassword(model.Password);

            var user = _context.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email == model.Email
                                  && x.PasswordHash == hashedPassword
                                  && x.IsActive);

            if (user == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                return View(model);
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role?.RoleName ?? "");

            if (model.RememberMe)
            {
                Response.Cookies.Append("RememberEmail", user.Email, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true
                });
            }

            if (user.Role?.RoleName == "Admin")
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            if (user.Role?.RoleName == "Staff")
                return RedirectToAction("Index", "Dashboard", new { area = "Staff" });

            TempData["Success"] = "Đăng nhập thành công";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExists = _context.Users.Any(x => x.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                return View(model);
            }

            int customerRoleId = _context.Roles.First(x => x.RoleName == "Customer").RoleId;

            User user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
                RoleId = customerRoleId,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            if (Request.Cookies["RememberEmail"] != null)
            {
                Response.Cookies.Delete("RememberEmail");
            }

            return RedirectToAction("Login");
        }
    }
}