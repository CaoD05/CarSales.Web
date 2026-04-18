namespace CarSales.Web.ViewModels;

public class ContentBlockViewModel
{
    public string Heading { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Items { get; set; } = [];
}
