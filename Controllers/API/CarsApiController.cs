using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers.API
{
    [Route("api/cars")]
    [ApiController]
    public class CarsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsApiController> _logger;

        public CarsApiController(
            ApplicationDbContext context,
            ILogger<CarsApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CarDto>>> GetAll(
            string? keyword,
            int? brandId,
            int? carTypeId,
            decimal? minPrice,
            decimal? maxPrice,
            int page = 1,
            int pageSize = 5)
        {
            try
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 5;

                var query = _context.Cars
                    .Include(x => x.Brand)
                    .Include(x => x.CarType)
                    .AsQueryable();

                // Apply filters
                query = ApplyFilters(query, keyword, brandId, carTypeId, minPrice, maxPrice);

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var cars = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => MapToCarDto(x))
                    .ToListAsync();

                var response = new PaginatedResponse<CarDto>
                {
                    Success = true,
                    Message = ApiMessages.CarListFetchedSuccessfully,
                    Filters = new FilterInfo
                    {
                        Keyword = keyword,
                        BrandId = brandId,
                        CarTypeId = carTypeId,
                        MinPrice = minPrice,
                        MaxPrice = maxPrice,
                        Page = page,
                        PageSize = pageSize
                    },
                    Pagination = new PaginationInfo
                    {
                        TotalItems = totalItems,
                        TotalPages = totalPages,
                        CurrentPage = page,
                        PageSize = pageSize
                    },
                    Data = cars
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cars list");
                return StatusCode(500, new ApiResponse<List<CarDto>>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CarDto>>> GetById(int id)
        {
            try
            {
                var car = await _context.Cars
                    .Include(x => x.Brand)
                    .Include(x => x.CarType)
                    .FirstOrDefaultAsync(x => x.CarId == id);

                if (car == null)
                {
                    return NotFound(new ApiResponse<CarDto>
                    {
                        Success = false,
                        Message = ApiMessages.CarNotFound
                    });
                }

                return Ok(new ApiResponse<CarDto>
                {
                    Success = true,
                    Message = ApiMessages.CarDetailFetchedSuccessfully,
                    Data = MapToCarDto(car)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting car with id {id}", id);
                return StatusCode(500, new ApiResponse<CarDto>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CarDto>>> Create([FromBody] CreateUpdateCarDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CreateUpdateCarDto>
                    {
                        Success = false,
                        Message = ApiMessages.InvalidData
                    });
                }

                var car = new Car
                {
                    CarName = model.CarName,
                    BrandId = model.BrandId,
                    CarTypeId = model.CarTypeId,
                    Price = model.Price,
                    Year = model.Year,
                    Mileage = model.Mileage,
                    FuelType = model.FuelType,
                    Transmission = model.Transmission,
                    Seats = model.Seats,
                    Color = model.Color,
                    Engine = model.Engine,
                    Origin = model.Origin,
                    Description = model.Description,
                    Thumbnail = model.Thumbnail,
                    Status = model.Status,
                    IsFeatured = model.IsFeatured,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();

                // Reload related entities
                await _context.Entry(car).Reference(x => x.Brand).LoadAsync();
                await _context.Entry(car).Reference(x => x.CarType).LoadAsync();

                _logger.LogInformation("Car created with id {id}", car.CarId);

                return CreatedAtAction(nameof(GetById), new { id = car.CarId },
                    new ApiResponse<CarDto>
                    {
                        Success = true,
                        Message = ApiMessages.CarCreatedSuccessfully,
                        Data = MapToCarDto(car)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating car");
                return StatusCode(500, new ApiResponse<CreateUpdateCarDto>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CarDto>>> Update(int id, [FromBody] CreateUpdateCarDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CreateUpdateCarDto>
                    {
                        Success = false,
                        Message = ApiMessages.InvalidData
                    });
                }

                var car = await _context.Cars
                    .Include(x => x.Brand)
                    .Include(x => x.CarType)
                    .FirstOrDefaultAsync(x => x.CarId == id);

                if (car == null)
                {
                    return NotFound(new ApiResponse<CarDto>
                    {
                        Success = false,
                        Message = ApiMessages.CarNotFound
                    });
                }

                car.CarName = model.CarName;
                car.BrandId = model.BrandId;
                car.CarTypeId = model.CarTypeId;
                car.Price = model.Price;
                car.Year = model.Year;
                car.Mileage = model.Mileage;
                car.FuelType = model.FuelType;
                car.Transmission = model.Transmission;
                car.Seats = model.Seats;
                car.Color = model.Color;
                car.Engine = model.Engine;
                car.Origin = model.Origin;
                car.Description = model.Description;
                car.Thumbnail = model.Thumbnail;
                car.Status = model.Status;
                car.IsFeatured = model.IsFeatured;
                car.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Car {id} updated", id);

                return Ok(new ApiResponse<CarDto>
                {
                    Success = true,
                    Message = ApiMessages.CarUpdatedSuccessfully,
                    Data = MapToCarDto(car)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating car with id {id}", id);
                return StatusCode(500, new ApiResponse<CarDto>
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
                var car = await _context.Cars.FindAsync(id);
                if (car == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = ApiMessages.CarNotFound
                    });
                }

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Car {id} deleted", id);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = ApiMessages.CarDeletedSuccessfully
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting car with id {id}", id);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        // Helper methods
        private IQueryable<Car> ApplyFilters(
            IQueryable<Car> query,
            string? keyword,
            int? brandId,
            int? carTypeId,
            decimal? minPrice,
            decimal? maxPrice)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.CarName.ToLower().Contains(lowerKeyword) ||
                    (x.Brand != null && x.Brand.BrandName.ToLower().Contains(lowerKeyword)) ||
                    (x.CarType != null && x.CarType.TypeName.ToLower().Contains(lowerKeyword))
                );
            }

            if (brandId.HasValue)
                query = query.Where(x => x.BrandId == brandId.Value);

            if (carTypeId.HasValue)
                query = query.Where(x => x.CarTypeId == carTypeId.Value);

            if (minPrice.HasValue)
                query = query.Where(x => x.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(x => x.Price <= maxPrice.Value);

            return query;
        }

        private static CarDto MapToCarDto(Car car)
        {
            return new CarDto
            {
                CarId = car.CarId,
                CarName = car.CarName,
                Price = car.Price,
                Year = car.Year,
                Mileage = car.Mileage,
                FuelType = car.FuelType,
                Transmission = car.Transmission,
                Seats = car.Seats,
                Color = car.Color,
                Engine = car.Engine,
                Origin = car.Origin,
                Description = car.Description,
                Thumbnail = car.Thumbnail,
                Status = car.Status,
                IsFeatured = car.IsFeatured,
                BrandId = car.BrandId,
                BrandName = car.Brand?.BrandName,
                CarTypeId = car.CarTypeId,
                CarTypeName = car.CarType?.TypeName
            };
        }
    }
}