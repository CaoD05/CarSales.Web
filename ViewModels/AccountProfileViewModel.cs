using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.ViewModels;

public class AccountProfileViewModel
{
    [Required(ErrorMessage = "Họ tên không được để trống")]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(255)]
    public string? Avatar { get; set; }
}
