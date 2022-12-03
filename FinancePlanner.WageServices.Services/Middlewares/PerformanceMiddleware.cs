using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FinancePlanner.WageServices.Services.Middlewares;

public class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMiddleware> _logger;

    public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        const int performanceTimeLog = 500;

        var sw = new Stopwatch();

        sw.Start();

        await _next(context);

        sw.Stop();

        if (performanceTimeLog < sw.ElapsedMilliseconds)
            _logger.LogWarning("Request {Method} {Path} took about {Elapsed} ms",
                context.Request.Method,
                context.Request.Path.Value,
                sw.ElapsedMilliseconds);
    }
}