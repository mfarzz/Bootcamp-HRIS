using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using MediatR;

namespace HRIS.API.Features.Employees.Commands.CreateDepartment;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : IRequest<Guid>;

public class CreateDepartementCommandHandler(IUnitOfWork uow) : IRequestHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Guid> Handle(CreateDepartmentCommand command, CancellationToken ct)
    {
        var department = command.Request.ToEntity();

        await uow.Departments.AddAsync(department, ct);
        await uow.SaveChangesAsync(ct);
        return department.Id;
    }
}