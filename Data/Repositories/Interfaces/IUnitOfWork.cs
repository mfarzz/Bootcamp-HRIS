namespace HRIS.API.Data.Repositories.Interfaces;

public interface IUnitOfWork
{
    IEmployeeRepository Employees { get; }
    IDepartmentRepository Departments { get; }
    IPositionRepository Positions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
