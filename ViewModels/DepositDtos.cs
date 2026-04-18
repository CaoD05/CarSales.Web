namespace CarSales.Web.ViewModels
{
    public class DepositDto
    {
        public int DepositId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int CarId { get; set; }
        public string? CarName { get; set; }
        public int? StaffId { get; set; }
        public string? StaffName { get; set; }
        public decimal DepositAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime DepositDate { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateDepositDto
    {
        public int CarId { get; set; }
        public decimal DepositAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Note { get; set; }
    }
}