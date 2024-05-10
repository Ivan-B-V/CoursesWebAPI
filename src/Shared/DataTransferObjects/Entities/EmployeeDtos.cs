namespace CoursesWebAPI.Shared.DataTransferObjects.Entities;

public record EmployeeDto : PersonDto
{

}

public record EmployeeForCreatingDto : PersonForCreatingDto
{
    public Guid? UserId { get; init; }
}

public record EmployeeForUpdateDto : PersonForUpdateDto
{

}
