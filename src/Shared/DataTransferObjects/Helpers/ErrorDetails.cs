namespace CoursesWebAPI.Shared.DataTransferObjects.Helpers;

public record ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}

public sealed record ResultDto<T>
{
    public ResultDto(T? value, bool isSucceeded, IEnumerable<ErrorDto> errors)
    {
        Value = isSucceeded ? value : default;
        IsSucceeded = isSucceeded;
        Errors = errors;
    }

    public T? Value { get; init; }
    public bool IsSucceeded { get; init; }
    public IEnumerable<ErrorDto> Errors { get; set; }
}

public sealed record ResultDto
{
    public ResultDto(bool isSucceeded, IEnumerable<ErrorDto> errors)
    {
        IsSucceeded = isSucceeded;
        Errors = errors;
    }

    public bool IsSucceeded { get; init; }
    public IEnumerable<ErrorDto> Errors { get; set; }
}

public sealed record ErrorDto
{
    public string Message { get; init; }

    public ErrorDto(string message)
    {
        Message = message;
    }
}
