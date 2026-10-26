using HRIS.API.Entities;
using HRIS.API.Models.Dtos;

namespace HRIS.API.Data.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IReadOnlyCollection<EmployeeResponse>> GetResponseAsync(CancellationToken ct = default);
    Task<EmployeeResponse?> GetResponseAsync(Guid id, CancellationToken ct = default);
    
    Task<IReadOnlyCollection<Employee>> GetDirectReportsAsync(Guid managerId, CancellationToken ct = default);
}