using GenkiSales.Shared.Requests;
using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Databases;
using GenkiSales.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenkiSales.WebApi.Services;

public class SalesOrderService(DataContext dataContext)
{
    public Task<List<SalesOrderResponse>> GetAll(DateTime startDate, DateTime endDate, Guid? customerId, CancellationToken cancellationToken)
    {
        var query = dataContext.SalesOrders
            .Where(x => x.OrderDate.Date >= startDate && x.OrderDate.Date <= endDate);

        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }

        return query
            .OrderByDescending(x => x.OrderNumber)
            .Select(x => new SalesOrderResponse
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                OrderDate = x.OrderDate,
                CustomerName = x.Customer!.Name,
                CustomerId = x.CustomerId,
                Total = x.Total
            }).ToListAsync(cancellationToken);
    }

    public async Task<SalesOrderResponse?> Get(Guid id, CancellationToken cancellationToken)
    {
        var salesOrder = await dataContext.SalesOrders
            .Where(x => x.Id == id)
            .Select(x => new SalesOrderResponse
            {
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                OrderDate = x.OrderDate,
                CustomerName = x.Customer!.Name,
                CustomerId = x.CustomerId,
                Total = x.Total
            }).FirstAsync(cancellationToken);

        salesOrder.Details = await dataContext.SalesOrderDetails
            .Where(x => x.SalesOrderId == id)
            .Select(x => new SalesOrderDetailResponse
            {
                Id = x.Id,
                ProductName = x.Product!.Name,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Price,
                Subtotal = x.Subtotal
            }).ToListAsync(cancellationToken);

        return salesOrder;
    }

    public async Task<Guid> Create(SalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrderCount = await dataContext.SalesOrders
            .Where(x => x.OrderDate.Year == request.OrderDate.Year)
            .CountAsync(cancellationToken);

        var salesOrder = new SalesOrder
        {
            Id = Guid.NewGuid(),
            OrderNumber = $"SO{request.OrderDate.Year}{salesOrderCount + 1:0000}",
            OrderDate = request.OrderDate,
            CustomerId = request.CustomerId,
        };
        dataContext.SalesOrders.Add(salesOrder);

        foreach (var detailRequest in request.Details)
        {
            var salesOrderDetail = new SalesOrderDetail
            {
                Id = Guid.NewGuid(),
                SalesOrderId = salesOrder.Id,
                ProductId = detailRequest.ProductId,
                Quantity = detailRequest.Quantity,
                Price = detailRequest.Price,
                Subtotal = detailRequest.Quantity * detailRequest.Price
            };
            dataContext.SalesOrderDetails.Add(salesOrderDetail);

            salesOrder.Total += salesOrderDetail.Subtotal;
        }

        await dataContext.SaveChangesAsync(cancellationToken);

        return salesOrder.Id;
    }

    public async Task Update(Guid id, SalesOrderRequest request, CancellationToken cancellationToken)
    {
        var oldDetails = await dataContext.SalesOrderDetails
            .Where(x => x.SalesOrderId == id)
            .ToListAsync(cancellationToken);

        dataContext.SalesOrderDetails.RemoveRange(oldDetails);

        var salesOrder = await dataContext.SalesOrders
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);

        salesOrder.OrderDate = request.OrderDate;
        salesOrder.CustomerId = request.CustomerId;
        salesOrder.Total = 0;

        foreach (var detailRequest in request.Details)
        {
            var salesOrderDetail = new SalesOrderDetail
            {
                Id = Guid.NewGuid(),
                SalesOrderId = salesOrder.Id,
                ProductId = detailRequest.ProductId,
                Quantity = detailRequest.Quantity,
                Price = detailRequest.Price,
                Subtotal = detailRequest.Quantity * detailRequest.Price
            };
            dataContext.SalesOrderDetails.Add(salesOrderDetail);

            salesOrder.Total += salesOrderDetail.Subtotal;
        }
        
        dataContext.SalesOrders.Update(salesOrder);

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var oldDetails = await dataContext.SalesOrderDetails
            .Where(x => x.SalesOrderId == id)
            .ToListAsync(cancellationToken);

        dataContext.SalesOrderDetails.RemoveRange(oldDetails);

        var salesOrder = await dataContext.SalesOrders
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);

        dataContext.SalesOrders.Remove(salesOrder);

        await dataContext.SaveChangesAsync(cancellationToken);
    }
}