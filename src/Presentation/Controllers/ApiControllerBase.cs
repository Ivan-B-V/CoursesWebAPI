using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ILogger logger;

    protected ApiControllerBase(ILogger logger)
    {
        this.logger = logger;
    }
}
