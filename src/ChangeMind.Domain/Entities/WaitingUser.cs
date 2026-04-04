namespace ChangeMind.Domain.Entities;

public class WaitingUser
{
    // Identifier
    public Guid Id { get; private set; }

    // Foreign Key
    public Guid UserId { get; private set; }

    // Primitive Properties
    public bool IsWaitingForAssignment { get; private set; } = true;

    // DateTime Properties
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    public User User { get; private set; }

    // EF Constructor
    private protected WaitingUser() { }

    /// <summary>
    /// Factory method to create a waiting user record
    /// </summary>
    public static WaitingUser Create(Guid userId)
    {
        return new WaitingUser
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            IsWaitingForAssignment = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Mark user as assigned to a training program
    /// </summary>
    public void MarkAsAssigned()
    {
        IsWaitingForAssignment = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark user as waiting again (e.g., if program is cancelled)
    /// </summary>
    public void MarkAsWaiting()
    {
        IsWaitingForAssignment = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
