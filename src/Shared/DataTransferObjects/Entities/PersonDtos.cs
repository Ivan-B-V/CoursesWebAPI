namespace CoursesWebAPI.Shared.DataTransferObjects.Entities;

public record PersonDto
{
    public Guid Id { get; init; }
    public required string FirsName { get; init; }
    public required string LastName { get; init; }
    public string? Patronomic { get; init; }
    public string? PhoneNumber { get; init; }
    public string Sex { get; init; } = string.Empty;
}

public record PersonForUserDto
{
    public Guid Id { get; init; }
    public required string FullName { get; init; }
    public string Sex { get; init; } = string.Empty;
}

public record PersonForCreatingDto
{
    public required string FirsName { get; init; }
    public required string LastName { get; init; }
    public string? Patronomic { get; init; }
    public string? PhoneNumber { get; init; }
    public string Sex { get; init; } = string.Empty;
}

public record PersonForUpdateDto
{
    public required string FirsName { get; init; }
    public required string LastName { get; init; }
    public string? Patronomic { get; init; }
    public string? PhoneNumber { get; init; }
    public string Sex { get; init; } = string.Empty;
}