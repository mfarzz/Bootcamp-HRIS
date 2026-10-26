using HRIS.API.Models.Dtos;

namespace HRIS.API.Entities;

public static class PositionMapping
{
    public static PositionResponse ToResponse(this Position position) =>
        new(
            position.Id,
            position.Title,
            position.Level
        );

    public static Position ToEntity(this CreatePositionRequest request) =>
        new(
            Guid.CreateVersion7(),
            request.Title,
            request.Level
            );
}