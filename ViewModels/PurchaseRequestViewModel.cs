using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.ViewModels
{
    public class PurchaseRequestViewModel
    {
        public int CarId { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Message { get; set; }
    }
}