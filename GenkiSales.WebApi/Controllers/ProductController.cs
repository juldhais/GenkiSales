using GenkiSales.Shared.Requests;
using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenkiSales.WebApi.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await productService.GetAll(cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productService.Get(id, cancellationToken);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(ProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await productService.Create(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, ProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await productService.Update(id, request, cancellationToken);
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
            await productService.Delete(id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
