using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/error-lab")]
public class ErrorLabController : ControllerBase
{
    [HttpGet("boom")]
    public IActionResult Boom()
        => throw new InvalidOperationException("Connection string 'Server=prod-sql-01;Password=s3cret' failed");

    [HttpGet("rethrow")]
    public IActionResult Rethrow(bool resetStack)
    {
        try
        {
            ReadEmployeeFile();
        }
        catch (InvalidOperationException ex)
        {
            if (resetStack)
            {
                throw ex;
            }

            throw;
        }
        return Ok();
    }

    private static void ReadEmployeeFile() => throw new InvalidOperationException("Employee file is locked");
}