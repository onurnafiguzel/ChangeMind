namespace ChangeMind.Domain.Entities;

public sealed class CoachUser
{
    // EF Core constructor — object creation only through factory method
    private CoachUser() { }

    public Guid Id { get; private set; }

    public Guid CoachId { get; private set; }
    public Guid UserId { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime AssignedAt { get; private set; }
    public DateTime? UnassignedAt { get; private set; }

    // Navigation Properties
    public Coach Coach { get; private set; } = null!;
    public User User { get; private set; } = null!;

    public static CoachUser Create(Guid coachId, Guid userId)
    {
        return new CoachUser
        {
            Id         = Guid.NewGuid(),
            CoachId    = coachId,
            UserId     = userId,
            IsActive   = true,
            AssignedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Ends the coach-user relationship. Safe to call multiple times (idempotent).
    /// </summary>
    public void Unassign()
    {
        if (!IsActive) return;
        IsActive      = false;
        UnassignedAt  = DateTime.UtcNow;
    }
}
