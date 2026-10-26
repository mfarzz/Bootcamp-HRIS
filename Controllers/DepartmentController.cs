using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Features.Departments.Queries.GetDepartmentTree;
using HRIS.API.Features.Employees.Commands.CreateDepartment;
using HRIS.API.Features.Employees.Queries.GetDepartments;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<DepartmentTreeDto>>> GetTree(CancellationToken ct)
        => Ok(await sender.Send(new GetDepartmentTreeQuery(), ct));

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateDepartmentRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateDepartmentCommand(request), ct);
        return Created($"/api/department/{id}", new {id});
    }
}