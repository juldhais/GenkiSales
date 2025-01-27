namespace GenkiSales.Shared.Responses;

public class SalesOrderDetailResponse
{
    public Guid Id { get; set; }
    public string? ProductName { get; set; }
    public Guid? ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal { get; set; }
}
