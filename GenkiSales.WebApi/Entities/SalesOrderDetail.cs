namespace GenkiSales.WebApi.Entities;

public class SalesOrderDetail
{
    public Guid Id { get; set; }
    public SalesOrder? SalesOrder { get; set; }
    public Guid? SalesOrderId { get; set; }
    public Product? Product { get; set; }
    public Guid? ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal { get; set; }
}