namespace ChangeMind.Domain.Exceptions;

/// <summary>
/// Thrown when authentication fails (e.g., invalid password, access denied).
/// Maps to HTTP 401 Unauthorized.
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException) { }
}
