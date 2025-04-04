using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Expenses.Api.Middleware;

public class LogRequestMiddleware(
    RequestDelegate next,
    ILogger<LogRequestMiddleware> logger,
    ICorrelationContextAccessor correlationContext)
{
        public async Task Invoke(HttpContext context)
    {
        var correlationId = correlationContext.CorrelationContext.CorrelationId;

        var sanitizedMethod = Sanitize(context.Request.Method);
        var sanitizedScheme = Sanitize(context.Request.Scheme);
        var sanitizedHost = Sanitize(context.Request.Host.ToString());
        var sanitizedPath = Sanitize(context.Request.Path.ToString());
        var sanitizedUrl = Sanitize(context.Request.GetDisplayUrl());

        logger.LogInformation(
            "Scheme: {scheme}, Host: {host}, Path: {path}, Method: {method}, url: {url}, correlationId: {correlationId}",
            sanitizedScheme, sanitizedHost, sanitizedPath, sanitizedMethod, sanitizedUrl,
            correlationId);

        await next(context);
    }

    private static string Sanitize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remplacer CRLF, tabulations, espaces multiples, etc.
        return input.Replace("\r", "")
                    .Replace("\n", "")
                    .Replace(Environment.NewLine, "")
                    .Replace("\t", "")
                    .Trim();
    }
}
