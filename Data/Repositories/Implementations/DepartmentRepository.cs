using HRIS.API.Data.Repositories.Interfaces;
using HRIS.API.Entities;
using HRIS.API.Models.Dtos;
using HRIS.API.Models.Mappings;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Data.Repositories.Implementations;

public class DepartmentRepository(HrisDbContext context) : Repository<Department>(context), IDepartmentRepository
{
    public async Task<bool> CodeExistsAsync(string code, CancellationToken ct = default)
        => await Context.Departments.AnyAsync(d => d.Code == code, ct);
}