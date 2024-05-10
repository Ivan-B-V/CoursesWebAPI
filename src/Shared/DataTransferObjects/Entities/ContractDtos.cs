
namespace CoursesWebAPI.Shared.DataTransferObjects.Entities;

public record ContractForCreatingDto
{
    public required string Number { get; init; }
    public decimal Cost { get; init; }
    public short Hours { get; init; }
    public short PaidHours { get; init; }
    public short PassedHours { get; init; }
    public bool Closed { get; init; }
    public Guid StudentId { get; init; }
}

public record ContractFullDto
{
    public Guid Id { get; init; }
    public required string Number { get; init; }
    public decimal Cost { get; init; }
    public short Hours { get; init; }
    public short PaidHours { get; init; }
    public short PassedHours { get; init; }
    public bool Closed { get; init; }
    public Guid StudentId { get; init; }
    public ICollection<Guid> ActivitiesIds { get; init; } = new HashSet<Guid>();
}

public record ContractForUpdateDto
{
    public required string Number { get; init; }
    public decimal Cost { get; init; }
    public short Hours { get; init; }
    public short PaidHours { get; init; }
    public short PassedHours { get; init; }
    public bool Closed { get; init; }
    public Guid StudentId { get; init; }
    public ICollection<Guid> ActivitiesIds { get; init; } = new HashSet<Guid>();
}
