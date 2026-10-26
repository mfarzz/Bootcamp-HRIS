using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Features.Employees.Commands.CreatePosition;
using HRIS.API.Features.Employees.Queries.GetPositions;
using HRIS.API.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PositionResponse>>> GetAllAsync(CancellationToken ct)
        => Ok(await sender.Send(new GetPositionsQuery(), ct));

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreatePositionRequest request, CancellationToken ct)
    {
        var id = sender.Send(new CreatePositionCommand(request), ct);
        return Created($"/api/position/{id}", new {id});
    }
}