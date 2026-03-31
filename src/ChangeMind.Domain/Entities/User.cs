namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class User
{
    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? Age { get; set; }
    /// <summary>
    /// Boy (cm)
    /// </summary>
    public decimal? Height { get; set; }
    /// <summary>
    /// Kilo (kg)
    /// </summary>
    public decimal? Weight { get; set; }
    public Gender? Gender { get; set; }
    public FitnessGoal? FitnessGoal { get; set; }
    public DifficultyLevel? FitnessLevel { get; set; }
    public bool IsActive { get; set; } = true;

    // DateTime Properties
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public ICollection<UserPhoto> Photos { get; set; } = new List<UserPhoto>();
    public ICollection<TrainingProgram> AssignedPrograms { get; set; } = new List<TrainingProgram>();
    public ICollection<CoachUser> CoachRelationships { get; set; } = new List<CoachUser>();
}
