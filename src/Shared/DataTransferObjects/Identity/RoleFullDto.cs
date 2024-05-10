using CoursesWebAPI.Core.Common.Enums;

namespace CoursesWebAPI.Shared.DataTransferObjects.Identity;

public record RoleFullDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public ICollection<ClaimDto> Claims { get; init; } = new HashSet<ClaimDto>();
    public ICollection<Permissions> Permissions { get; init; } = new HashSet<Permissions>();
}

public record RoleDto
{
    public string Name { get; init; } = string.Empty;
}

public record RoleForCreatingDto
{
    public string Name { get; init; } = string.Empty;

    public ICollection<ClaimDto> Claims { get; init; } = new HashSet<ClaimDto>();
    public ICollection<Permissions> Permissions { get; init; } = new HashSet<Permissions>();
}

public record RoleForUpdateDto
{
    public string Name {get; init;} = string.Empty;

    public ICollection<ClaimDto> Claims { get; init; } = new HashSet<ClaimDto>();
    public ICollection<Permissions> Permissions { get; init; } = new HashSet<Permissions>();
}

public record ClaimDto
{
    public string Type { get; init;} = string.Empty;
    public string Value { get; init;} = string.Empty;
};
