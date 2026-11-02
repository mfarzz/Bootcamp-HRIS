using HRIS.API.Common.Exceptions;
using HRIS.API.Data;
using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Entities;
using HRIS.API.Extensions;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(CreateEmployeeRequest Request) : IRequest<Guid>;

public class CreateEmployeeCommandHandler(
    IUnitOfWork uow,
    TimeProvider timeProvider,
    ILogger<CreateEmployeeCommandHandler> logger) : IRequestHandler<CreateEmployeeCommand, Guid>
{
    public async Task<Guid> Handle(CreateEmployeeCommand command, CancellationToken ct)
    {
        var request = command.Request with { Email = command.Request.Email.NormalizeEmail() };

        if (await uow.Employees.EmployeeNumberExistsAsync(request.EmployeeNumber, ct))
        {
            throw new DuplicateEmployeeException($"Employee {request.EmployeeNumber} already exists");
        }

        if (await uow.Employees.EmailExistsAsync(request.Email, ct))
        {
            throw new DuplicateEmployeeException("This email already exists");
        }
        
        var employee = command.Request.ToEntity(timeProvider.GetUtcNow(), EmployeeStatus.Active);
        await uow.Employees.AddAsync(employee, ct);

        try
        {
            await uow.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when(ex.IsUniqueConstrainViolation())
        {
            throw new DuplicateEmployeeException($"Employee number or email already exists", ex);
        }
        
        logger.LogInformation("Employee {EmployeeId} created", employee.Id);
        return employee.Id;
    }
}