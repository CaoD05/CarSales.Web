using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(255)]
        public string? Avatar { get; set; }

        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Role? Role { get; set; }
        public virtual ICollection<Favorite>? Favorites { get; set; }
        public virtual ICollection<PurchaseRequest>? PurchaseRequests { get; set; }
        public virtual ICollection<Deposit>? Deposits { get; set; }
    }
}