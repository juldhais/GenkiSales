using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Databases;
using Microsoft.EntityFrameworkCore;

namespace GenkiSales.WebApi.Services;

public class SalesReportService(DataContext dataContext)
{
    public async Task<List<SalesReportResponse>> GetReport(DateTime startDate, DateTime endDate, Guid? customerId, Guid? productId, CancellationToken cancellationToken)
    {
        var query = dataContext.SalesOrderDetails
            .Where(x => x.SalesOrder!.OrderDate.Date >= startDate.Date && x.SalesOrder.OrderDate.Date <= endDate.Date);

        if (customerId != null)
        {
            query = query.Where(x => x.SalesOrder!.CustomerId == customerId);
        }

        if (productId != null)
        {
            query = query.Where(x => x.ProductId == productId);
        }

        var result = await query
            .OrderByDescending(x => x.SalesOrder!.OrderDate)
            .Select(x => new SalesReportResponse
            {
                OrderNumber = x.SalesOrder!.OrderNumber,
                OrderDate = x.SalesOrder.OrderDate,
                CustomerName = x.SalesOrder.Customer!.Name,
                ProductName = x.Product!.Name,
                Quantity = x.Quantity,
                Price = x.Price,
                Subtotal = x.Quantity * x.Price
            }).ToListAsync(cancellationToken);

        return result;
    }
}
