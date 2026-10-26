namespace HRIS.API.Models.Dtos;

public record DepartmentResponse(
    Guid Id,
    string Code,
    string Name,
    Guid? ParentDepartmentId);
    
public record CreateDepartmentRequest(
    string Code,
    string Name,
    Guid? ParentDepartmentId);
    