using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Common.ExceptionHandlers;

public static class ProblemDetailWriter
{
    public const string ErrorCodeKey = "errorCode";

    public static async ValueTask WriteAsync(
        IProblemDetailsService service, HttpContext httpContext, ProblemDetails problem,
        Exception exception, CancellationToken ct
    )
    {
        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

        var writer = await service.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception,
        });

        if (!writer)
            await httpContext.Response.WriteAsJsonAsync(problem, ct);
    }
}