namespace CarSales.Web.ViewModels
{
    public class PurchaseRequestDto
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int CarId { get; set; }
        public string? CarName { get; set; }
        public int? StaffId { get; set; }
        public string? StaffName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Message { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreatePurchaseRequestDto
    {
        public int CarId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Message { get; set; }
    }

    public class UpdatePurchaseRequestStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}