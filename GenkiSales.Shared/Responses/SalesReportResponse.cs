namespace GenkiSales.Shared.Responses;

public class SalesReportResponse
{
    public string? OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal { get; set; }
}