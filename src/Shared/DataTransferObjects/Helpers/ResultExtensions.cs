using FluentResults;

namespace CoursesWebAPI.Shared.DataTransferObjects.Helpers;

public static class ResultExtensions
{
    public static ResultDto<T> ToResultDto<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new ResultDto<T>(result.Value, true, Enumerable.Empty<ErrorDto>());
        }
        var errors = result.Errors.Select(error => new ErrorDto(error.Message));
        return new ResultDto<T>(default, false, errors);
    }

    public static ResultDto ToResultDto(this Result result)
    {
        if (result.IsSuccess)
        {
            return new ResultDto( true, Enumerable.Empty<ErrorDto>());
        }
        var errors = result.Errors.Select(error => new ErrorDto(error.Message));
        return new ResultDto(false, errors);
    }
}
