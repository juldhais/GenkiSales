namespace GenkiSales.Shared.Requests;

public class SalesOrderDetailRequest
{
    public Guid? ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
}