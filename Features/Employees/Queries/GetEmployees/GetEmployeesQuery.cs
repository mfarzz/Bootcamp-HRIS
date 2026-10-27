using HRIS.API.Common.Paging;
using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(
    string? Search, Guid? DepartmentId, EmployeeStatus? EmployeeStatus, string? SortBy, bool Desc = false, int Page = 1, int PageSize = 10
    ) : IRequest<PagedResult<EmployeeListDto>>;

public record EmployeeListDto(
    Guid Id,
    string EmployeeNumber,
    string FullName,
    string DepartmentName,
    string PositionTitle,
    string? ManagerName,
    EmployeeStatus EmployeeStatus
);

public class GetEmployeesQueryHandler(HrisDbContext db) : IRequestHandler<GetEmployeesQuery, PagedResult<EmployeeListDto>>
{
    public async Task<PagedResult<EmployeeListDto>> Handle(GetEmployeesQuery query, CancellationToken ct)
    {
        IQueryable<Employee> employees = db.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            employees = employees.Where(e=> e.FullName.Contains(query.Search) || e.EmployeeNumber.Contains(query.Search));
        }

        if (query.DepartmentId is not null)     
        {
            employees = employees.Where(e=> e.DepartmentId == query.DepartmentId);
        }

        var status = query.EmployeeStatus ?? EmployeeStatus.Active;
        employees = employees.Where(e => e.EmployeeStatus == status);

        employees = query.SortBy?.ToLowerInvariant() switch
        {
            "hiredate" => query.Desc
                ? employees.OrderByDescending(e => e.HireDate).ThenBy(e => e.EmployeeNumber)
                : employees.OrderBy(e => e.HireDate).ThenBy(e => e.EmployeeNumber),
            "number" => query.Desc
                ? employees.OrderByDescending(e => e.EmployeeNumber)
                : employees.OrderBy(e => e.EmployeeNumber),
            _ => query.Desc
                ? employees.OrderByDescending(e => e.FullName).ThenBy(e => e.EmployeeNumber)
                : employees.OrderBy(e => e.FullName).ThenBy(e => e.EmployeeNumber)
        };

        return await employees
            .Select(e => new EmployeeListDto(
                e.Id,
                e.EmployeeNumber,
                e.FullName,
                e.Department.Name,
                e.Position.Title,
                e.Manager != null ? e.Manager.FullName : null,
                e.EmployeeStatus
            )).ToPagedResultAsync(query.Page, query.PageSize, ct:ct);
    }
}