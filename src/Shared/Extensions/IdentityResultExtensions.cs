using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace CoursesWebAPI.Shared.Extensions
{
    public static class IdentityResultExtensions
    {
        public static Result ToResult(this IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                return Result.Fail(identityResult.Errors.Select(error => $"Code: {error.Code}, Description: {error.Description}."));
            }
            return Result.Ok();
        }
    }
}
