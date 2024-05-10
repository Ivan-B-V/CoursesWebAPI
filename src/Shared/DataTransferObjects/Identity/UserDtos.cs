using CoursesWebAPI.Shared.DataTransferObjects.Entities;

namespace CoursesWebAPI.Shared.DataTransferObjects.Identity;

public record UserDto
{
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public bool IsLocked { get; init; }
    public bool EmailConfirmed { get; init; }
    public string? PhoneNumber { get; init; }
    public PersonForUserDto? Person { get; init; }
    public ICollection<RoleDto> Roles { get; init; } = new HashSet<RoleDto>();
}

public record UserForRegistrationDto
{
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public Guid? PersonId { get; init; }
}

public record UserWithEmployeeForRegistrationDto : UserForRegistrationDto
{
    public required EmployeeForCreatingDto EmployeeForCreating { get; init; }
}

public record UserForUpdateDto
{
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public Guid? PersonId { get; init; }
    public IEnumerable<string> Roles { get; init; } = new HashSet<string>();
}
