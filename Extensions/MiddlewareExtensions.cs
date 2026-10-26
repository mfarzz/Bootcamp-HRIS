using HRIS.API.Middlewares;

namespace HRIS.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestTimingMiddleware>();
    
    public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder builder)
        => builder.UseMiddleware<MaintenanceModeMiddleware>();

    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        builder.UseRequestTiming();
        builder.UseMaintenanceMode();
        return builder;
    }

    public static IServiceCollection AddHrisMiddleware(this IServiceCollection services)
    {
        services.AddSingleton<RequestTimingMiddleware>();
        services.AddSingleton<MaintenanceModeMiddleware>();
        
        return services;
    }
}