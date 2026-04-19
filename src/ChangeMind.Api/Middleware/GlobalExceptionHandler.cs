namespace ChangeMind.Api.Middleware;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // 400 Validation errors — list of field errors
            ValidationException validationException => new ValidationProblemDetails(
                validationException.Errors
                    .Select((e, i) => new { Key = i.ToString(), Value = e })
                    .ToDictionary(x => x.Key, x => new[] { x.Value }))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = validationException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 400 Bad Request
            BadRequestException badRequestException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = badRequestException.Message,
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

            // 404 Not Found
            NotFoundException notFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = notFoundException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 404 Not Found (KeyNotFoundException — legacy handler uyumluluğu)
            KeyNotFoundException keyNotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = keyNotFoundException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 409 Conflict — DuplicateEmailException, DuplicateIdempotencyKeyException ve diğerleri
            ConflictException conflictException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = conflictException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 409 Conflict — DB unique constraint violation (idempotency son güvence)
            DbUpdateException dbEx when dbEx.InnerException?.Message.Contains("23505") == true
                || dbEx.InnerException?.Message.Contains("UX_Payments_UserId_IdempotencyKey") == true
                => new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = "A payment with this idempotency key has already been processed.",
                    Instance = httpContext.Request.Path,
                    Type = "https://tools.ietf.org/html/rfc7807"
                },

            // 400 Bad Request (InvalidOperationException — framework hataları)
            InvalidOperationException invalidOpException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = invalidOpException.Message,
                Instance = httpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7807"
            },

            // 500 Internal Server Error (fallback)
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
