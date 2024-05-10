namespace CoursesWebAPI.Shared.DataTransferObjects.Entities;

public record ActivityForCreatingDto
{
    public string? Description { get; init; }
    public DateTimeOffset Begin { get; init; }
    public DateTimeOffset End { get; init; }
    public Guid ActivityTypeId { get; init; }
    public Guid? TeacherId { get; init; }
    public ICollection<Guid> ContractsIds { get; init; } = new HashSet<Guid>();
}

public record ActivityFullDto
{
    public Guid Id { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset Begin { get; init; }
    public DateTimeOffset End { get; init; }
    public required string ActivityType { get; init; }
    public string? Teacher { get; init; }
    public ICollection<string> ContractsNumber { get; init; } = new HashSet<string>();
}

public record ActivityForUpdateDto
{
    public string? Description { get; init; }
    public DateTimeOffset Begin { get; init; }
    public DateTimeOffset End { get; init; }
    public Guid ActivityTypeId { get; init; }
    public Guid? TeacherId { get; init; }
    public ICollection<Guid> ContractsIds { get; init; } = new HashSet<Guid>();
}
