namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class Coach
{
    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CoachSpecialization? Specialization { get; set; }
    public bool IsActive { get; set; } = true;

    // DateTime Properties
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<CoachUser> AssignedUsers { get; set; } = new List<CoachUser>();
    public ICollection<TrainingProgram> CreatedPrograms { get; set; } = new List<TrainingProgram>();
}
