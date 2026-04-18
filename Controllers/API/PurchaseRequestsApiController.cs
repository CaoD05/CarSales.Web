using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers.API
{
    [Route("api/purchase-requests")]
    [ApiController]
    public class PurchaseRequestsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRequestsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _context.PurchaseRequests
                .Include(x => x.Car)
                .Include(x => x.User)
                .Include(x => x.Staff)
                .Select(x => new
                {
                    x.RequestId,
                    x.UserId,
                    UserName = x.User != null ? x.User.FullName : null,
                    x.CarId,
                    CarName = x.Car != null ? x.Car.CarName : null,
                    x.StaffId,
                    StaffName = x.Staff != null ? x.Staff.FullName : null,
                    x.FullName,
                    x.Email,
                    x.Phone,
                    x.Message,
                    x.Status,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _context.PurchaseRequests
                .Include(x => x.Car)
                .Include(x => x.User)
                .Include(x => x.Staff)
                .Where(x => x.RequestId == id)
                .Select(x => new
                {
                    x.RequestId,
                    x.UserId,
                    UserName = x.User != null ? x.User.FullName : null,
                    x.CarId,
                    CarName = x.Car != null ? x.Car.CarName : null,
                    x.StaffId,
                    StaffName = x.Staff != null ? x.Staff.FullName : null,
                    x.FullName,
                    x.Email,
                    x.Phone,
                    x.Message,
                    x.Status,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (request == null)
                return NotFound(new { message = "Không tìm thấy yêu cầu mua" });

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (customer == null)
                return BadRequest(new { message = "Không tìm thấy khách hàng tương ứng với email" });

            var car = await _context.Cars.FindAsync(model.CarId);
            if (car == null)
                return BadRequest(new { message = "Xe không tồn tại" });

            var request = new PurchaseRequest
            {
                UserId = customer.UserId,
                CarId = model.CarId,
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Message = model.Message,
                Status = "New",
                CreatedAt = DateTime.Now
            };

            _context.PurchaseRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo yêu cầu mua thành công",
                data = request
            });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var request = await _context.PurchaseRequests.FindAsync(id);
            if (request == null)
                return NotFound(new { message = "Không tìm thấy yêu cầu mua" });

            request.Status = status;
            request.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật trạng thái thành công",
                data = request
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.PurchaseRequests.FindAsync(id);
            if (request == null)
                return NotFound(new { message = "Không tìm thấy yêu cầu mua" });

            _context.PurchaseRequests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa yêu cầu mua thành công" });
        }
    }
}