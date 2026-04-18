using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class CarType
    {
        public int CarTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string TypeName { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        public virtual ICollection<Car>? Cars { get; set; }
    }
}