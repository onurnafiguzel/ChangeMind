namespace ChangeMind.Domain.Exceptions;

/// <summary>
/// Thrown when an authenticated user attempts an action they are not permitted to perform.
/// Maps to HTTP 403 Forbidden.
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}
