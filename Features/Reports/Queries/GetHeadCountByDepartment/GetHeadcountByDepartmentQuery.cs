using HRIS.API.Data;
using HRIS.API.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Reports.Queries.GetHeadCountByDepartment;

public record HeadCountByDepartmentDto(string DepartmentCode, string DepartmentName, int HeadCount);

public class GetHeadcountByDepartmentQuery : IRequest<IReadOnlyCollection<HeadCountByDepartmentDto>>;

public class GetHeadcountByDepartmentHandler(HrisDbContext db) 
    :  IRequestHandler<GetHeadcountByDepartmentQuery, IReadOnlyCollection<HeadCountByDepartmentDto>>
{
    public async Task<IReadOnlyCollection<HeadCountByDepartmentDto>> Handle(GetHeadcountByDepartmentQuery query, CancellationToken ct)
        => await db.Employees
            .AsNoTracking()
            .Where(e=> e.EmployeeStatus == EmployeeStatus.Active)
            .GroupBy(e=> new {e.Department.Code, e.Department.Name})
            .OrderByDescending(g=> g.Count()).ThenBy(g=> g.Key.Code)
            .Select(g=> new HeadCountByDepartmentDto(g.Key.Code, g.Key.Name, g.Count()))
            .ToListAsync(ct);
}