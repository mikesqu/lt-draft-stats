using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract information from HttpContext
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var referer = context.Request.Headers["Referer"].ToString();
        var origin = context.Request.Headers["Origin"].ToString();
        var accept = context.Request.Headers["Accept"].ToString();
        var xforward = context.Request.Headers["X-Forwarded-For"].ToString();
        var requestedUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

        // Log the information
        _logger.LogInformation("Request from IP: {ClientIp}, URL: {RequestedUrl}, User-Agent: {UserAgent}, Referer: {referer}, Origin: {origin}, Accept: {accept}, XForward: {xforward}",
            clientIp, requestedUrl, userAgent, referer, origin, accept, xforward);

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
