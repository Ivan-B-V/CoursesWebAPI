using CoursesWebAPI.Core.Entities;

namespace CoursesWebAPI.Shared.DataTransferObjects.Entities;

public record StudentDto : PersonDto
{

}

public record StudentForCreatingDto : PersonForCreatingDto
{

}

public record StudentForUpdateDto : PersonForUpdateDto
{
    public ICollection<Contract> Contracts { get; init; } = new HashSet<Contract>();
}
