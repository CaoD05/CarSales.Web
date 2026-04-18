using CarSales.Web.Data;
using CarSales.Web.Helpers;
using CarSales.Web.Models;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers.API
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string hashedPassword = PasswordHelper.HashPassword(model.Password);

            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == model.Email
                                       && x.PasswordHash == hashedPassword
                                       && x.IsActive);

            if (user == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công",
                data = new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    Role = user.Role != null ? user.Role.RoleName : null
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool emailExists = await _context.Users.AnyAsync(x => x.Email == model.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "Email đã tồn tại" });
            }

            int customerRoleId = await _context.Roles
                .Where(x => x.RoleName == "Customer")
                .Select(x => x.RoleId)
                .FirstAsync();

            var user = new User
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
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đăng ký thành công",
                data = new
                {
                    user.UserId,
                    user.FullName,
                    user.Email
                }
            });
        }
    }
}