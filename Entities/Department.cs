namespace HRIS.API.Entities;

public class Department
{
    public Department(Guid id, string code, string name, Guid? parentDepartmentId)
    {
        Id = id;
        Code = code;
        Name = name;
        ParentDepartmentId = parentDepartmentId;
    }

    public Department() { }                                // for object initializers (HasData, tests); EF Core calls the full constructor

    public Guid Id { get; set; }
    public string Code { get; set; } = null!;              // unique, e.g. "ENG"
    public string Name { get; set; } = null!;

    public Guid? ParentDepartmentId { get; set; }          // nullable: a root department has no parent
    public Department? ParentDepartment { get; set; }      // navigation to the parent
    public ICollection<Department> SubDepartments { get; set; } = [];   // navigation to the children

    public ICollection<Employee> Employees { get; set; } = [];
}