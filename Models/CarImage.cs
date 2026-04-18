using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class CarImage
    {
        [Key]
        public int ImageId { get; set; }

        public int CarId { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; } = false;
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public virtual Car? Car { get; set; }
    }
}