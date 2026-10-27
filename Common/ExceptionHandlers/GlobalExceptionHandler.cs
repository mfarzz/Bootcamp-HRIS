using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Common.ExceptionHandlers;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService
    ) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        logger.LogError(exception, "Unhandled {ExceptionType} whiele processing {Method} {Path}",
            exception.GetType(), httpContext.Request.Method, httpContext.Request.Path);

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occured",
            Detail = "Please contact support and quote the tracerId"
        };
        if (problem is ValidationProblemDetails && !problem.Extensions.ContainsKey(ProblemDetailWriter.ErrorCodeKey))
        {
            problem.Extensions[ProblemDetailWriter.ErrorCodeKey] = "MALFORMED_REQUEST";
        }
        problem.Extensions[ProblemDetailWriter.ErrorCodeKey] = "INTERNAL_ERROR";


        await ProblemDetailWriter.WriteAsync(problemDetailsService, httpContext, problem, exception, ct);
        return true;
    }
}