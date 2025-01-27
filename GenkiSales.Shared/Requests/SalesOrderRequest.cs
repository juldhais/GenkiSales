namespace GenkiSales.Shared.Requests;

public class SalesOrderRequest
{
    public DateTime OrderDate { get; set; }
    public Guid? CustomerId { get; set; }
    public List<SalesOrderDetailRequest> Details { get; set; } = [];
}