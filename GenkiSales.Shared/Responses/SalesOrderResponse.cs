namespace GenkiSales.Shared.Responses;

public class SalesOrderResponse
{
    public Guid Id { get; set; }
    public string? OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string? CustomerName { get; set; }
    public Guid? CustomerId { get; set; }
    public decimal Total { get; set; }
    public List<SalesOrderDetailResponse> Details { get; set; } = [];
}
