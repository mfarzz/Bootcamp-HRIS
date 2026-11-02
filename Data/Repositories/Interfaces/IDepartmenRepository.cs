using HRIS.API.Entities;
using HRIS.API.Models.Dtos;

namespace HRIS.API.Data.Repositories.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<bool> CodeExistsAsync(string code, CancellationToken ct = default);
}