using CoursesWebAPI.Application.Contracts.IdentityServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CoursesWebAPI.Presentation.ActionFilters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class CustomAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly IApplicationAuthorizationService authorizationService;

    public CustomAuthorizationAttribute(IApplicationAuthorizationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var actionName = context.RouteData?.Values["action"]?.ToString()
            ?? throw new ArgumentException("actionName");

        //var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
        //if (string.IsNullOrEmpty(authorizationHeader))
        //{
        //    context.Result = new UnauthorizedResult();
        //    return;
        //}
        //var jwt = authorizationHeader.ToString()
        //                             .Replace("bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);
        ////var method = context.HttpContext.Request.Method;
        ////var endPoint = context.HttpContext.Request.Path;
        //var validationResult = authorizationService.ValidateJwt(jwt);
        //if (validationResult.IsFailed)
        //{
        //    context.Result = new UnauthorizedResult();
        //    return;
        //}
        //var userId = validationResult.Value.Claims.First(c => c.Type.Equals("id")).Value;
        var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Sub))?.Value;
        if (userId is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        var result = await authorizationService.AuthorizeUserByIdAsync(new Guid(userId), actionName);
        if (result.IsFailed)
        {
            context.Result = new ForbidResult();
        }
    }
}
