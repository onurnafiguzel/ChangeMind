namespace ChangeMind.Domain.Exceptions;

/// <summary>
/// Thrown when an email address is already registered in the system.
/// Maps to HTTP 409 Conflict.
/// </summary>
public class DuplicateEmailException : ConflictException
{
    public string Email { get; }

    public DuplicateEmailException(string email)
        : base($"The email address '{email}' is already registered.")
    {
        Email = email;
    }
}
