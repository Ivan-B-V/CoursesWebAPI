using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using Microsoft.AspNetCore.Diagnostics;

namespace WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, Serilog.ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        //UnauthorizedException => StatusCodes.Status401Unauthorized,
                        //NotFoundException => StatusCodes.Status404NotFound,
                        //BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };
                    logger.Error("Error occurred: {error}.\nTraceId: {traceId}.\nUsername: {username}", 
                        contextFeature.Error, context.TraceIdentifier, context.User.Identity?.Name);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(
                        new ErrorDetails
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                            })
                    .ConfigureAwait(false);
                }
            });
        });
    }
}
