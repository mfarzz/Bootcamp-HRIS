using HRIS.API.Data.Repositories.Interfaces;
using MediatR;

namespace HRIS.API.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid Id) : IRequest<bool>;

public class DeleteEmployeeCommandHandler(IUnitOfWork uow, ILogger<DeleteEmployeeCommandHandler> logger) :  IRequestHandler<DeleteEmployeeCommand, bool>
{
    public async Task<bool> Handle(DeleteEmployeeCommand command, CancellationToken ct)
    {
        var employee = await uow.Employees.GetByIdAsync(command.Id, ct);
        if (employee is null)
            return false;
        
        uow.Employees.Delete(employee);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}