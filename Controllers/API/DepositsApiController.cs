using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers.API
{
    [Route("api/deposits")]
    [ApiController]
    public class DepositsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepositsApiController> _logger;

        public DepositsApiController(
            ApplicationDbContext context,
            ILogger<DepositsApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DepositDto>>>> GetAll(int page = 1, int pageSize = 10)
        {
            try
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                var deposits = await _context.Deposits
                    .Include(x => x.Car)
                    .Include(x => x.User)
                    .Include(x => x.Staff)
                    .OrderByDescending(x => x.DepositDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => MapToDepositDto(x))
                    .ToListAsync();

                return Ok(new ApiResponse<List<DepositDto>>
                {
                    Success = true,
                    Message = ApiMessages.DepositListFetchedSuccessfully,
                    Data = deposits
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deposits");
                return StatusCode(500, new ApiResponse<List<DepositDto>>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DepositDto>>> GetById(int id)
        {
            try
            {
                var deposit = await _context.Deposits
                    .Include(x => x.Car)
                    .Include(x => x.User)
                    .Include(x => x.Staff)
                    .FirstOrDefaultAsync(x => x.DepositId == id);

                if (deposit == null)
                {
                    return NotFound(new ApiResponse<DepositDto>
                    {
                        Success = false,
                        Message = ApiMessages.DepositNotFound
                    });
                }

                return Ok(new ApiResponse<DepositDto>
                {
                    Success = true,
                    Message = ApiMessages.DepositDetailFetchedSuccessfully,
                    Data = MapToDepositDto(deposit)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deposit with id {id}", id);
                return StatusCode(500, new ApiResponse<DepositDto>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DepositDto>>> Create([FromBody] CreateDepositDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CreateDepositDto>
                    {
                        Success = false,
                        Message = ApiMessages.InvalidData
                    });
                }

                var car = await _context.Cars.FindAsync(model.CarId);
                if (car == null)
                {
                    return BadRequest(new ApiResponse<CreateDepositDto>
                    {
                        Success = false,
                        Message = ApiMessages.CarNotFound
                    });
                }

                var currentUser = await _context.Users.FirstOrDefaultAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new ApiResponse<CreateDepositDto>
                    {
                        Success = false,
                        Message = ApiMessages.UnauthorizedAction
                    });
                }

                var deposit = new Deposit
                {
                    UserId = currentUser.UserId,
                    CarId = model.CarId,
                    DepositAmount = model.DepositAmount,
                    PaymentMethod = model.PaymentMethod,
                    Note = model.Note,
                    DepositDate = DateTime.UtcNow,
                    Status = "Pending",
                    PaymentStatus = "Pending"
                };

                _context.Deposits.Add(deposit);
                await _context.SaveChangesAsync();

                await _context.Entry(deposit).Reference(x => x.Car).LoadAsync();
                await _context.Entry(deposit).Reference(x => x.User).LoadAsync();

                _logger.LogInformation("Deposit created with id {id}", deposit.DepositId);

                return CreatedAtAction(nameof(GetById), new { id = deposit.DepositId },
                    new ApiResponse<DepositDto>
                    {
                        Success = true,
                        Message = ApiMessages.DepositCreatedSuccessfully,
                        Data = MapToDepositDto(deposit)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating deposit");
                return StatusCode(500, new ApiResponse<CreateDepositDto>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpPut("{id}/confirm")]
        public async Task<ActionResult<ApiResponse<DepositDto>>> ConfirmDeposit(int id)
        {
            try
            {
                var deposit = await _context.Deposits
                    .Include(x => x.Car)
                    .Include(x => x.User)
                    .Include(x => x.Staff)
                    .FirstOrDefaultAsync(x => x.DepositId == id);

                if (deposit == null)
                {
                    return NotFound(new ApiResponse<DepositDto>
                    {
                        Success = false,
                        Message = ApiMessages.DepositNotFound
                    });
                }

                deposit.Status = "Confirmed";
                deposit.PaymentStatus = "Completed";

                await _context.SaveChangesAsync();

                _logger.LogInformation("Deposit {id} confirmed", id);

                return Ok(new ApiResponse<DepositDto>
                {
                    Success = true,
                    Message = ApiMessages.DepositConfirmedSuccessfully,
                    Data = MapToDepositDto(deposit)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming deposit with id {id}", id);
                return StatusCode(500, new ApiResponse<DepositDto>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                var deposit = await _context.Deposits.FindAsync(id);
                if (deposit == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = ApiMessages.DepositNotFound
                    });
                }

                _context.Deposits.Remove(deposit);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deposit {id} deleted", id);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = ApiMessages.DepositDeletedSuccessfully
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting deposit with id {id}", id);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        private static DepositDto MapToDepositDto(Deposit deposit)
        {
            return new DepositDto
            {
                DepositId = deposit.DepositId,
                UserId = deposit.UserId,
                UserName = deposit.User?.FullName,
                CarId = deposit.CarId,
                CarName = deposit.Car?.CarName,
                StaffId = deposit.StaffId,
                StaffName = deposit.Staff?.FullName,
                DepositAmount = deposit.DepositAmount,
                PaymentMethod = deposit.PaymentMethod,
                PaymentStatus = deposit.PaymentStatus,
                DepositDate = deposit.DepositDate,
                Note = deposit.Note,
                Status = deposit.Status
            };
        }
    }
}