namespace HRIS.API.Entities;

public class Position
{
    public Position(Guid id, string title, int level)
    {
        Id = id;
        Title = title;
        Level = level;
    }

    public Position() { }

    public Guid Id { get; set; }
    public string Title { get; set; } = null!;             // unique, e.g. "Software Engineer"
    public int Level { get; set; }                          // higher = more senior (seed: Staff 2, Supervisor 5, Department Head 8)
    public ICollection<Employee> Employees { get; set; } = [];
}