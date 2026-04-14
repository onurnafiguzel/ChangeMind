namespace ChangeMind.Domain.Exceptions;

/// <summary>
/// Thrown when an entity transition to a new state is not valid given its current state.
/// Maps to HTTP 409 Conflict.
/// </summary>
public class InvalidStateTransitionException : ConflictException
{
    public InvalidStateTransitionException(string entityName, string currentState, string attemptedAction)
        : base($"Cannot perform '{attemptedAction}' on {entityName} in state '{currentState}'.")
    {
    }

    public InvalidStateTransitionException(string message)
        : base(message)
    {
    }
}
