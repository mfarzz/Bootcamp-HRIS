using HRIS.API.Data.Repositories.Implementations;
using HRIS.API.Entities;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using MediatR;

namespace HRIS.API.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(CreateEmployeeRequest Request) : IRequest<Guid>;

public class CreateEmployeeCommandHandler(
    UnitOfWork uow,
    TimeProvider timeProvider,
    ILogger<CreateEmployeeCommandHandler> logger) : IRequestHandler<CreateEmployeeCommand, Guid>
{
    public async Task<Guid> Handle(CreateEmployeeCommand command, CancellationToken ct)
    {
        var employee = command.Request.ToEntity(timeProvider.GetUtcNow(), EmployeeStatus.Active);
        await uow.Employees.AddAsync(employee, ct);
        await uow.SaveChangesAsync(ct);
        
        logger.LogInformation("Employee {EmployeeId} created", employee.Id);
        return employee.Id;
    }
}