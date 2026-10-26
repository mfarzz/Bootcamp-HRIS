using HRIS.API.Data.Repositories.Implementations;
using HRIS.API.Entities;
using HRIS.API.Features.Employees.Commands.CreateEmployee;
using HRIS.API.Models.Dtos;
using MediatR;

namespace HRIS.API.Features.Employees.Commands.CreatePosition;

public record CreatePositionCommand(CreatePositionRequest Request) : IRequest<Guid>;

public class CreatePositionCommandHandler(UnitOfWork uow) : IRequestHandler<CreatePositionCommand, Guid>
{
    public async Task<Guid> Handle(CreatePositionCommand command, CancellationToken ct)
    {
        var positon = command.Request.ToEntity();

        await uow.Positions.AddAsync(positon, ct);
        await uow.SaveChangesAsync(ct);
        return positon.Id;
    }
}