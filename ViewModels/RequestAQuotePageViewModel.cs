namespace CarSales.Web.ViewModels;

public class RequestAQuotePageViewModel
{
    public int SelectedCarId { get; set; }
    public RequestAQuoteCarOptionViewModel? SelectedCar { get; set; }
    public bool HasOpenPurchaseRequest { get; set; }
    public List<RequestAQuoteCarOptionViewModel> Cars { get; set; } = [];
}

public class RequestAQuoteCarOptionViewModel
{
    public int CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Thumbnail { get; set; }
}
