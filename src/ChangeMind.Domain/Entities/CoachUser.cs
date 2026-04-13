namespace ChangeMind.Domain.Entities;

public class CoachUser
{
    private CoachUser() { }

    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public bool IsActive { get; set; } = true;

    // DateTime Properties
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UnassignedAt { get; set; }

    // Foreign Keys
    public Guid CoachId { get; set; }
    public Guid UserId { get; set; }

    // Navigation Properties
    public Coach Coach { get; set; } = null!;
    public User User { get; set; } = null!;

    public static CoachUser Create(Guid coachId, Guid userId)
    {
        return new CoachUser
        {
            Id = Guid.NewGuid(),
            CoachId = coachId,
            UserId = userId,
            IsActive = true,
            AssignedAt = DateTime.UtcNow
        };
    }

    public void Unassign()
    {
        IsActive = false;
        UnassignedAt = DateTime.UtcNow;
    }
}
