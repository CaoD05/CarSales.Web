using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Tên hãng không được để trống")]
        [StringLength(100)]
        public string BrandName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? Logo { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Car>? Cars { get; set; }
    }
}