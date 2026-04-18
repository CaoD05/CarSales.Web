using CarSales.Web.Models;

namespace CarSales.Web.ViewModels
{
    public class UserTicketsViewModel
    {
        public List<PurchaseRequest> PurchaseRequests { get; set; } = new();
        public List<Deposit> Deposits { get; set; } = new();
    }
}
