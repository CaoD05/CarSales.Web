using CarSales.Web.Models;

namespace CarSales.Web.ViewModels
{
    public class CarBuyPageViewModel
    {
        public Car Car { get; set; } = null!;
        public decimal SuggestedDepositAmount { get; set; }
        public bool HasOpenPurchaseRequest { get; set; }
        public bool HasOpenDeposit { get; set; }
    }
}
