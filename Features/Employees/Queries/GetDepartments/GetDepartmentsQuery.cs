using HRIS.API.Data;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Employees.Queries.GetDepartments;

public record GetDepartmentsQuery : IRequest<IReadOnlyCollection<DepartmentResponse>>;

public class GetDepartmentsQueryHandler(HrisDbContext db) : IRequestHandler<GetDepartmentsQuery, IReadOnlyCollection<DepartmentResponse>>
{
    public async Task<IReadOnlyCollection<DepartmentResponse>> Handle(GetDepartmentsQuery query, CancellationToken ct)
        => await db.Departments
            .AsNoTracking()
            .OrderBy(d => d.Code)
            .Select(d => d.ToResponse())
            .ToListAsync(ct);
}