using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Entities;
using MediatR;

namespace HRIS.API.Features.Employees.Commands.TerminateEmployee;

public enum TerminateResult {Terminated, NotFound, AlreadyTerminated}

public record TerminateEmployeeCommand(Guid EmployeeId) : IRequest<TerminateResult>;

public class TerminatedEmployeeCommandHandler(IUnitOfWork uow, TimeProvider timeProvider, ILogger<TerminatedEmployeeCommandHandler> logger) : IRequestHandler<TerminateEmployeeCommand, TerminateResult>
{
    public async Task<TerminateResult> Handle(TerminateEmployeeCommand command, CancellationToken ct)
    {
        var employee = await uow.Employees.GetByIdAsync(command.EmployeeId, ct);
        if (employee is null)
            return TerminateResult.NotFound;

        if (employee.EmployeeStatus == EmployeeStatus.Terminated)
            return TerminateResult.AlreadyTerminated;

        employee.EmployeeStatus = EmployeeStatus.Terminated;
        employee.TerminatedAt = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

        var directReports = await uow.Employees.GetDirectReportsAsync(employee.Id, ct);
        foreach (var report in directReports)
        {
            report.ManagerId = employee.ManagerId;
        }

        await uow.SaveChangesAsync(ct);
        
        logger.LogInformation("Employee {EmployeeId} terminated; {ReassignedCount} direct report move to manager {ManagerId}",
            employee.Id, directReports.Count, employee.ManagerId
            );
        return TerminateResult.Terminated;
    }
}