using GenkiSales.Shared.Requests;
using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Databases;
using GenkiSales.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenkiSales.WebApi.Services;

public class ProductService(DataContext dataContext)
{
    public Task<List<ProductResponse>> GetAll(CancellationToken cancellationToken)
    {
        return dataContext.Products
            .Where(x => x.IsDeleted == false)
            .Select(x => new ProductResponse
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price
            }).ToListAsync(cancellationToken);
    }

    public Task<ProductResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        return dataContext.Products
            .Where(x => x.Id == id)
            .Select(x => new ProductResponse
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price
            }).FirstAsync(cancellationToken);
    }

    public async Task<Guid> Create(ProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };
        dataContext.Products.Add(product);

        await dataContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    public async Task Update(Guid id, ProductRequest request, CancellationToken cancellationToken)
    {
        var product = await dataContext.Products
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);

        product.Name = request.Name;
        product.Price = request.Price;
        dataContext.Products.Update(product);

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var product = await dataContext.Products
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);

        product.IsDeleted = true;
        dataContext.Products.Update(product);

        await dataContext.SaveChangesAsync(cancellationToken);
    }
}