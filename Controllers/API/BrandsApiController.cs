using CarSales.Web.Data;
using CarSales.Web.Models;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers.API
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BrandsApiController> _logger;

        public BrandsApiController(
            ApplicationDbContext context,
            ILogger<BrandsApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BrandDto>>>> GetAll(int page = 1, int pageSize = 10)
        {
            try
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                var brands = await _context.Brands
                    .OrderBy(x => x.BrandName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => MapToBrandDto(x))
                    .ToListAsync();

                return Ok(new ApiResponse<List<BrandDto>>
                {
                    Success = true,
                    Message = ApiMessages.BrandListFetchedSuccessfully,
                    Data = brands
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting brands");
                return StatusCode(500, new ApiResponse<List<BrandDto>>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BrandDto>>> GetById(int id)
        {
            try
            {
                var brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == id);

                if (brand == null)
                {
                    return NotFound(new ApiResponse<BrandDto>
                    {
                        Success = false,
                        Message = ApiMessages.BrandNotFound
                    });
                }

                return Ok(new ApiResponse<BrandDto>
                {
                    Success = true,
                    Message = ApiMessages.BrandDetailFetchedSuccessfully,
                    Data = MapToBrandDto(brand)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting brand with id {id}", id);
                return StatusCode(500, new ApiResponse<BrandDto>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BrandDto>>> Create([FromBody] CreateUpdateBrandDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CreateUpdateBrandDto>
                    {
                        Success = false,
                        Message = ApiMessages.InvalidData
                    });
                }

                var brand = new Brand
                {
                    BrandName = model.BrandName,
                    Country = model.Country,
                    Description = model.Description,
                    Logo = model.Logo,
                    IsActive = model.IsActive
                };

                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Brand created with id {id}", brand.BrandId);

                return CreatedAtAction(nameof(GetById), new { id = brand.BrandId },
                    new ApiResponse<BrandDto>
                    {
                        Success = true,
                        Message = ApiMessages.BrandCreatedSuccessfully,
                        Data = MapToBrandDto(brand)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating brand");
                return StatusCode(500, new ApiResponse<CreateUpdateBrandDto>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<BrandDto>>> Update(int id, [FromBody] CreateUpdateBrandDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CreateUpdateBrandDto>
                    {
                        Success = false,
                        Message = ApiMessages.InvalidData
                    });
                }

                var brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                {
                    return NotFound(new ApiResponse<BrandDto>
                    {
                        Success = false,
                        Message = ApiMessages.BrandNotFound
                    });
                }

                brand.BrandName = model.BrandName;
                brand.Country = model.Country;
                brand.Description = model.Description;
                brand.Logo = model.Logo;
                brand.IsActive = model.IsActive;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Brand {id} updated", id);

                return Ok(new ApiResponse<BrandDto>
                {
                    Success = true,
                    Message = ApiMessages.BrandUpdatedSuccessfully,
                    Data = MapToBrandDto(brand)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating brand with id {id}", id);
                return StatusCode(500, new ApiResponse<BrandDto>
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
                var brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = ApiMessages.BrandNotFound
                    });
                }

                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Brand {id} deleted", id);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = ApiMessages.BrandDeletedSuccessfully
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting brand with id {id}", id);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ApiMessages.ServerError
                });
            }
        }

        private static BrandDto MapToBrandDto(Brand brand)
        {
            return new BrandDto
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                Country = brand.Country,
                Description = brand.Description,
                Logo = brand.Logo,
                IsActive = brand.IsActive
            };
        }
    }
}