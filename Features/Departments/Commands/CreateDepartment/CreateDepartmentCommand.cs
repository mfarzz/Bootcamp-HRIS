using Azure.Core;
using HRIS.API.Common.Exceptions;
using HRIS.API.Data;
using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Extensions;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : IRequest<Guid>;

public class CreateDepartmentCommandHandler(IUnitOfWork uow) : IRequestHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Guid> Handle(CreateDepartmentCommand command, CancellationToken ct)
    {
        var request = command.Request with { Code = command.Request.Code.NormalizeCode() };
        if (await uow.Departments.CodeExistsAsync(request.Code, ct))
        {
            throw new DuplicateDepartmentException($"Department with code {request.Code} already exists");
        }
        
        var department = command.Request.ToEntity();
        await uow.Departments.AddAsync(department, ct);

        try
        {
            await uow.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when(ex.IsUniqueConstrainViolation())
        {
            throw new DuplicateDepartmentException($"Department code is already used", ex);
        }
        
        return department.Id;
    }
}