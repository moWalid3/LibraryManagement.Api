using System.Diagnostics;

namespace LibraryManagement.Api.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var stopWatch = Stopwatch.StartNew();
        httpContext.Response.OnStarting(() => // this callback function will run when the response is about to be sent
        {
            stopWatch.Stop();
            httpContext.Response.Headers["X-Response-Time-Ms"] = stopWatch.ElapsedMilliseconds.ToString();
            return Task.CompletedTask;
        });

        await _next(httpContext);
    }
}
