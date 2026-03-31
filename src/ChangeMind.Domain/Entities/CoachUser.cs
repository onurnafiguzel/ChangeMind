namespace ChangeMind.Domain.Entities;

public class CoachUser
{
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
    public Coach Coach { get; set; }
    public User User { get; set; }
}
