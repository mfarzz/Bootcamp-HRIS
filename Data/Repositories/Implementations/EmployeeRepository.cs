using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Entities;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Data.Repositories.Implementations;

public class EmployeeRepository(HrisDbContext context) : Repository<Employee>(context), IEmployeeRepository
{
    public async Task<IReadOnlyCollection<Employee>> GetDirectReportsAsync(Guid managerId, CancellationToken ct = default)
        => await Context.Employees.Where(e => e.ManagerId == managerId).ToListAsync(ct);

    public async Task<bool> EmployeeNumberExistsAsync(string employeeNumber, CancellationToken ct = default)
        => await Context.Employees.AnyAsync(e => e.EmployeeNumber == employeeNumber, ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await Context.Employees.AnyAsync(e => e.Email == email, ct);
}