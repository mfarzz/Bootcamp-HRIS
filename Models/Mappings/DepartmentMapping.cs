using HRIS.API.Entities;
using HRIS.API.Models.Dtos;

namespace HRIS.API.Models.Mappings;

public static class DepartmentMapping
{
    public static DepartmentResponse ToResponse(this Department department ) =>
        new(
            department.Id,
            department.Code,
            department.Name,
            department.ParentDepartmentId );

    public static Department ToEntity(this CreateDepartmentRequest request) =>
        new(
            Guid.CreateVersion7(),
            request.Code,
            request.Name,
            request.ParentDepartmentId
            );
}