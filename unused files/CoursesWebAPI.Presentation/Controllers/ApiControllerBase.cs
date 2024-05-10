using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly ILogger logger;

        protected ApiControllerBase(ILogger logger)
        {
            this.logger = logger;
        }
    }
}
