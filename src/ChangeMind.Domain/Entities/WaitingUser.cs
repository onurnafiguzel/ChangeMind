namespace ChangeMind.Domain.Entities;

public sealed class WaitingUser
{
    // EF Core constructor — object creation only through factory method
    private protected WaitingUser() { }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public bool IsWaitingForAssignment { get; private set; } = true;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    public User User { get; private set; } = null!;

    public static WaitingUser Create(Guid userId)
    {
        return new WaitingUser
        {
            Id                     = Guid.NewGuid(),
            UserId                 = userId,
            IsWaitingForAssignment = true,
            CreatedAt              = DateTime.UtcNow,
            UpdatedAt              = null
        };
    }

    /// <summary>
    /// Records that a coach has been assigned and a training program created.
    /// </summary>
    public void MarkAsAssigned()
    {
        IsWaitingForAssignment = false;
        UpdatedAt              = DateTime.UtcNow;
    }

    /// <summary>
    /// Re-queues the user for coach assignment (e.g. if a program is cancelled).
    /// </summary>
    public void MarkAsWaiting()
    {
        IsWaitingForAssignment = true;
        UpdatedAt              = DateTime.UtcNow;
    }
}
