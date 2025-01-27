using GenkiSales.Shared.Requests;
using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Databases;
using GenkiSales.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenkiSales.WebApi.Services;

public class CustomerService(DataContext dataContext)
{
    public Task<List<CustomerResponse>> GetAll(CancellationToken cancellationToken)
    {
        return dataContext.Customers
            .Where(x => x.IsDeleted == false)
            .Select(x => new CustomerResponse
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email
            }).ToListAsync(cancellationToken);
    }

    public Task<CustomerResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        return dataContext.Customers
            .Where(x => x.Id == id)
            .Select(x => new CustomerResponse
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email
            }).FirstAsync(cancellationToken);
    }

    public async Task<Guid> Create(CustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email
        };
        dataContext.Customers.Add(customer);

        await dataContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }

    public async Task Update(Guid id, CustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await dataContext.Customers
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);

        customer.Name = request.Name;
        customer.Address = request.Address;
        customer.Phone = request.Phone;
        customer.Email = request.Email;
        dataContext.Customers.Update(customer);

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var customer = await dataContext.Customers
            .Where(x => x.Id == id)
            .FirstAsync(cancellationToken);

        customer.IsDeleted = true;
        dataContext.Customers.Update(customer);

        await dataContext.SaveChangesAsync(cancellationToken);
    }
}