using GenkiSales.Shared.Requests;
using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenkiSales.WebApi.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(CustomerService customerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await customerService.GetAll(cancellationToken);
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await customerService.Get(id, cancellationToken);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await customerService.Create(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, CustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await customerService.Update(id, request, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await customerService.Delete(id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}