using System.Security.Claims;
using System.Text.Json.Serialization;
using HRIS.API.Extensions;
using HRIS.API.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting HRIS API");
    var builder = WebApplication.CreateBuilder(args); // Konfigurasi project

    // Add services to the container.
    builder.AddHrisSerilog();
    builder.Services.AddControllers()
        .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    builder.Services.AddHrisService(builder.Configuration);
    builder.Services.AddHrisMiddleware();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build(); // Sebuah pipeline, merupakan sebuah middleware

    app.UseRequestTiming();
    app.UseHrisRequestLogging();
    app.UseExceptionHandler();
    app.UseMaintenanceMode();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUI(option => { option.SwaggerEndpoint("/openapi/v1.json", "HRIS API v1"); });
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.Use(async (context, next) =>
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        using (Serilog.Context.LogContext.PushProperty("UserId", userId))
        {
            await next();
        }
    });

    app.UseAuthorization();

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception e) when (e is not HostAbortedException)
{
    Log.Fatal(e, "HRIS API terminated unexpectedly");
}
finally
{
    Log.Information("Stopping HRIS API");
    Log.CloseAndFlush();
}

