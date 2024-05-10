namespace CoursesWebAPI.Shared.DataTransferObjects.Entities;

public sealed record ActivityTypeDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}

public sealed record ActivityTypeForUpdateDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}

public sealed record ActivityTypeForCreatingDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
