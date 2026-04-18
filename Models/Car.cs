using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class Car
    {
        public int CarId { get; set; }

        [Required(ErrorMessage = "Tên xe không được để trống")]
        [StringLength(150)]
        public string CarName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hãng xe không được để trống")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Loại xe không được để trống")]
        public int CarTypeId { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(1, 99999999999)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Năm sản xuất không được để trống")]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Range(0, int.MaxValue)]
        public int? Mileage { get; set; }

        [StringLength(50)]
        public string? FuelType { get; set; }

        [StringLength(50)]
        public string? Transmission { get; set; }

        [Range(1, 100)]
        public int? Seats { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(100)]
        public string? Engine { get; set; }

        [StringLength(100)]
        public string? Origin { get; set; }

        public string? Description { get; set; }

        [StringLength(255)]
        public string? Thumbnail { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Available";

        public bool IsFeatured { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual CarType? CarType { get; set; }
        public virtual ICollection<CarImage>? CarImages { get; set; }
    }
}