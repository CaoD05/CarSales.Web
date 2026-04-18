using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class PurchaseRequest
    {
        [Key]
        public int RequestId { get; set; }

        public int UserId { get; set; }
        public int CarId { get; set; }
        public int? StaffId { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Message { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "New";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public virtual User? User { get; set; }
        public virtual User? Staff { get; set; }
        public virtual Car? Car { get; set; }
    }
}