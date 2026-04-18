using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.ViewModels
{
    public class DepositViewModel
    {
        public int CarId { get; set; }

        [Required]
        [Range(1000000, 9999999999)]
        public decimal DepositAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Note { get; set; }
    }
}