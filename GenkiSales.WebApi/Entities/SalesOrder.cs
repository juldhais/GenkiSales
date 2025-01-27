namespace GenkiSales.WebApi.Entities;

public class SalesOrder
{
    public Guid Id { get; set; }
    public string? OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public Customer? Customer { get; set; }
    public Guid? CustomerId { get; set; }
    public decimal Total { get; set; }
}
