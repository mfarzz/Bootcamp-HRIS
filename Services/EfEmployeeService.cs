using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Entities;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;

namespace HRIS.API.Services;

public class EfEmployeeService(IUnitOfWork uow, TimeProvider timeProvider, ILogger<EfEmployeeService> logger) : IEmployeeService
{
    public async Task<IReadOnlyCollection<EmployeeResponse>> GetAllAsync(CancellationToken ct = default)
        => await uow.Employees.GetResponseAsync(ct);

    public async Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await uow.Employees.GetResponseAsync(id, ct);

    public async Task<Guid> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
    {
        var employee = request.ToEntity(
            timeProvider.GetUtcNow(),
            EmployeeStatus.Active
        );
        
        await uow.Employees.AddAsync(employee, ct);
        await uow.SaveChangesAsync(ct);
        logger.LogInformation("Employee {EmployeeId} created", employee.Id);
        return employee.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default)
    {
        var employee = await uow.Employees.GetByIdAsync(id, ct);
        if (employee is null)
            return false;
        
        employee.ApplyUpdateEntity(request);
        await uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var employee = await uow.Employees.GetByIdAsync(id, ct);
        if (employee is null)
            return false;
        
        uow.Employees.Delete(employee);
        await uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<TerminateResult> TerminateAsync(Guid id, CancellationToken ct = default)
    {
        // check employee if exist
        var employee = await uow.Employees.GetByIdAsync(id, ct);
        if (employee is null)
            return TerminateResult.NotFound;
        
        // change status(terminated) & catat waktu terminate (TerminatedAt)
        if (employee.Status == EmployeeStatus.Terminated)
            return TerminateResult.AlreadyTerminated;
        
        employee.Status = EmployeeStatus.Terminated;
        employee.TerminatedAt = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        
        // every direct report moves up to terminated employee manager | may be null
        var directReport = await uow.Employees.GetDirectReportsAsync(employee.Id, ct);

        foreach (var report in directReport)
        {
            report.ManagerId = employee.ManagerId;
        }
        
        // save changes employee
        await uow.SaveChangesAsync(ct);
        logger.LogInformation("Employee {EmployeeId} is terminated; {ReassignedCount} direct reports move to manager {ManagerId}", 
            id, employee.Id, directReport.Count);

        return TerminateResult.Terminated;
    }
}