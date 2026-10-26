namespace HRIS.API.Models.Dtos;

public record PositionResponse(
    Guid Id,
    string Title,
    int Level
    );
    
public record CreatePositionRequest(
    string Title,
    int Level
);
    