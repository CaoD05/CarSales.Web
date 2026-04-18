using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty;

        public virtual ICollection<User>? Users { get; set; }
    }
}