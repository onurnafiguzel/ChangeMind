namespace ChangeMind.Domain.Exceptions;

/// <summary>
/// Thrown when one or more validation rules fail.
/// Maps to HTTP 400 Bad Request.
/// </summary>
public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string error)
        : base("One or more validation errors occurred.")
    {
        Errors = [error];
    }
}
