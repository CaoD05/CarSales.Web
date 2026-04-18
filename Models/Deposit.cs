using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models
{
    public class Deposit
    {
        [Key]
        public int DepositId { get; set; }

        public int UserId { get; set; }
        public int CarId { get; set; }
        public int? StaffId { get; set; }

        [Required]
        [Range(1000000, 9999999999)]
        public decimal DepositAmount { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Pending";

        public DateTime DepositDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Note { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "WaitingConfirm";

        public virtual User? User { get; set; }
        public virtual User? Staff { get; set; }
        public virtual Car? Car { get; set; }
    }
}