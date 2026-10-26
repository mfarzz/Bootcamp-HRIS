using HRIS.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController(
    ITransactionOperation transaction,
    IScopeOperation scope,
    ISingletonOperation singleton,
    OperationReport report
    ) : ControllerBase
{
    [HttpGet("lifetime")]
    public IActionResult GetLifetime() => Ok(new
    {
        Transient = new { Controller = transaction.OperaionId, Helper = report.Transaction.OperaionId },
        Scoped = new { Controller = scope.OperaionId, Helper = report.Transaction.OperaionId },
        Singleton = new { Controller = singleton.OperaionId, Helper = report.Transaction.OperaionId }
    });
}