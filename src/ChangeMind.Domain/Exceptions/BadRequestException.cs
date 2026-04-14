namespace ChangeMind.Domain.Exceptions;

/// <summary>
/// Thrown when the request is malformed or contains invalid data.
/// Maps to HTTP 400 Bad Request.
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException) { }
}
