using GenkiSales.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenkiSales.WebApi.Databases;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
}
