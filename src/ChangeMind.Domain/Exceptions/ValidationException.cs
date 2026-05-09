namespace ChangeMind.Domain.Exceptions;

/// <summary>
/// Thrown when one or more validation rules fail.
/// Maps to HTTP 400 Bad Request.
/// </summary>
public class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string error)
        : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>
        {
            [""] = [error]
        };
    }
}
