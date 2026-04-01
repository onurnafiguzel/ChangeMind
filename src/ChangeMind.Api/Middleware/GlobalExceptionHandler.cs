namespace ChangeMind.Api.Middleware;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChangeMind.Domain.Exceptions;

/// <summary>
/// Global exception handler that converts all exceptions to standardized ProblemDetails responses.
/// Implements IExceptionHandler for ASP.NET Core 10+ unified exception handling.
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var problemDetails = MapExceptionToProblemDetails(exception, httpContext);

        httpContext.Response.StatusCode = problemDetails.Status ?? 500;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails MapExceptionToProblemDetails(Exception exception, HttpContext httpContext)
    {
        return exception switch
        {
            // 404 Not Found
            NotFoundException notFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = notFoundException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 409 Conflict
            ConflictException conflictException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = conflictException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 401 Unauthorized
            UnauthorizedException unauthorizedException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = unauthorizedException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 400 Bad Request (InvalidOperationException, validation errors)
            InvalidOperationException invalidOpException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = invalidOpException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 404 Not Found (for KeyNotFoundException)
            KeyNotFoundException keyNotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = keyNotFoundException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 500 Internal Server Error (fallback for all other exceptions)
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred. Please try again later.",
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            }
        };
    }
}
