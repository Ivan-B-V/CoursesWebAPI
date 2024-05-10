namespace CoursesWebAPI.Shared.RequestFeatures;

public abstract class RequestQueryParameters
{
    private const int maxPageSize = 100;
    private int pageSize = 20;
    private int pageNumber = 1;

    public int PageNumber
    {
        get => pageNumber;
        set => pageNumber = value < 0 ? 1 : value;
    }

    public int PageSize
    {
        get => pageSize;
        set => pageSize = value > maxPageSize || value < 0 ? maxPageSize : value;
    }

    public string? OrderBy { get; set; }

    public string? SearchTerm { get; set; }
}

public class UserQueryParameters : RequestQueryParameters
{
    public UserQueryParameters()
    {
        OrderBy = "username";
    }

    public string? Role { get; init; }
}

public class ActivityQueryParameters : RequestQueryParameters
{
    public ActivityQueryParameters()
    {
        OrderBy = "Begin";
    }

    public string ActivityType { get; set; } = string.Empty; 

    public DateTimeOffset From { get; set; } = default;

    public DateTimeOffset To { get; set; } = default;

}

public class PersonQueryParameters : RequestQueryParameters
{
    public PersonQueryParameters()
    {
        OrderBy = "LastName,FirstName";
    }
}

public class ContractQueryParameters : RequestQueryParameters
{
    public ContractQueryParameters() => OrderBy = "number";

    public string? Name { get; set; }
}
