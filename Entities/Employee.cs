namespace HRIS.API.Entities;

public enum EmployeeStatus
{
    Active,
    Terminated
}

public class Employee
{
    public Employee(Guid id, string employeeNumber, string fullName, string email, DateOnly hireDate,
        DateTimeOffset createdAt, EmployeeStatus status, DateOnly? terminatedAt,
        Guid departmentId, Guid positionId, Guid? managerId)
    {
        Id = id;
        EmployeeNumber = employeeNumber;
        FullName = fullName;
        Email = email;
        HireDate = hireDate;
        CreatedAt = createdAt;
        Status = status;
        TerminatedAt = terminatedAt;
        DepartmentId = departmentId;
        PositionId = positionId;
        ManagerId = managerId;
    }

    public Employee() { }                                   // for object initializers (HasData, tests); EF Core calls the full constructor

    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; } = null!;     // unique, e.g. "EMP-0001"
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;              // unique
    public DateOnly HireDate { get; set; }                  // DateOnly maps to SQL Server 'date'
    public EmployeeStatus Status { get; set; }                // no default here: the service sets it (Lesson 4)
    public DateTimeOffset CreatedAt { get; set; }           // set by the server from TimeProvider, never by the client
    public DateOnly? TerminatedAt { get; set; }

    public Guid DepartmentId { get; set; }                  // required relationship: non-nullable FK
    public Department Department { get; set; } = null!;

    public Guid PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public Guid? ManagerId { get; set; }                    // optional self-reference: the direct manager
    public Employee? Manager { get; set; }
    public ICollection<Employee> DirectReports { get; set; } = [];
}