using HRIS.API.Entities;
using HRIS.API.Models.Dtos;

namespace HRIS.API.Data.Repositories.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IReadOnlyCollection<DepartmentResponse>> GetResponseAsync(CancellationToken ct = default);
}