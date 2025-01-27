using GenkiSales.Shared.Requests;
using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenkiSales.WebApi.Controllers;

[Route("api/sales-orders")]
[ApiController]
public class SalesOrderController(SalesOrderService salesOrderService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SalesOrderResponse>>> GetAll(DateTime startDate, DateTime endDate, Guid? customerId, CancellationToken cancellationToken)
    {
        var salesorders = await salesOrderService.GetAll(startDate, endDate, customerId, cancellationToken);
        return Ok(salesorders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SalesOrderResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var salesorder = await salesOrderService.Get(id, cancellationToken);
            return Ok(salesorder);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(SalesOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await salesOrderService.Create(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, SalesOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await salesOrderService.Update(id, request, cancellationToken);
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
            await salesOrderService.Delete(id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}