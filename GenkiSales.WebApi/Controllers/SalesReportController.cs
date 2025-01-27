using GenkiSales.Shared.Responses;
using GenkiSales.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenkiSales.WebApi.Controllers;

[Route("api/sales-reports")]
[ApiController]
public class SalesReportController(SalesReportService salesReportService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SalesReportResponse>>> GetReport(DateTime startDate, DateTime endDate, Guid? customerId, Guid? productId, CancellationToken cancellationToken)
    {
        var salesOrderReport = await salesReportService.GetReport(startDate, endDate, customerId, productId, cancellationToken);
        return Ok(salesOrderReport);
    }
}
