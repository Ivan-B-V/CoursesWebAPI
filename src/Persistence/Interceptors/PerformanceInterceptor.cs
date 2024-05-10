using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using System.Data.Common;
using System.Diagnostics;

namespace CoursesWebAPI.Persistence.Interceptors;

public sealed class PerformanceInterceptor : DbCommandInterceptor
{
    private const long QuerySlowThreshold = 100; //in ms
    private readonly ILogger logger;

    public PerformanceInterceptor(ILogger logger)
    {
        this.logger = logger;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        var originResult =  base.ReaderExecuting(command, eventData, result);
        stopwatch.Stop();
        if (stopwatch.ElapsedMilliseconds > QuerySlowThreshold)
        {
            logger.Warning("Slow Query Detected: {command}", command.CommandText);
        }
        return originResult;
    }
}

