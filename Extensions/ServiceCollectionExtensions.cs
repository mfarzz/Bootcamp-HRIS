using HRIS.API.Common.Behaviors;
using HRIS.API.Data;
using HRIS.API.Data.Repositories;
using HRIS.API.Data.Repositories.Implementations;
using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Middlewares;
using HRIS.API.Services;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHrisService(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        var connectionString = configuration.GetConnectionString("HrisDb");
        services.AddDbContext<HrisDbContext>(options => options.UseSqlServer(connectionString));

        services.AddSingleton(TimeProvider.System);

        services.AddTransient<ITransactionOperation, Operation>();
        services.AddScoped<IScopeOperation, Operation>();
        services.AddSingleton<ISingletonOperation, Operation>();
        services.AddScoped<OperationReport>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        return services;
    }
}