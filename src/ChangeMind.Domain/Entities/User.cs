namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class User
{
    // Identifier
    public Guid Id { get; private set; }

    // Primitive Properties
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public int? Age { get; private set; }
    /// <summary>
    /// Boy (cm)
    /// </summary>
    public decimal? Height { get; private set; }
    /// <summary>
    /// Kilo (kg)
    /// </summary>
    public decimal? Weight { get; private set; }
    public Gender? Gender { get; private set; }
    public FitnessGoal? FitnessGoal { get; private set; }
    public DifficultyLevel? FitnessLevel { get; private set; }
    public bool IsActive { get; private set; } = true;
    public UserRole Role { get; private set; } = UserRole.User;

    // DateTime Properties
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    private readonly List<UserPhoto> _photos = new();
    public IReadOnlyCollection<UserPhoto> Photos => _photos.AsReadOnly();

    private readonly List<TrainingProgram> _assignedPrograms = new();
    public IReadOnlyCollection<TrainingProgram> AssignedPrograms => _assignedPrograms.AsReadOnly();

    private readonly List<CoachUser> _coachRelationships = new();
    public IReadOnlyCollection<CoachUser> CoachRelationships => _coachRelationships.AsReadOnly();

    // EF Constructor
    private protected User() { }

    /// <summary>
    /// Factory method to create a new User
    /// </summary>
    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        int? age = null,
        decimal? height = null,
        decimal? weight = null,
        Gender? gender = null,
        FitnessGoal? fitnessGoal = null,
        DifficultyLevel? fitnessLevel = null)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            Age = age,
            Height = height,
            Weight = weight,
            Gender = gender,
            FitnessGoal = fitnessGoal,
            FitnessLevel = fitnessLevel,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Update user profile information
    /// </summary>
    public void Update(
        string firstName,
        string lastName,
        int? age = null,
        decimal? height = null,
        decimal? weight = null,
        Gender? gender = null,
        FitnessGoal? fitnessGoal = null,
        DifficultyLevel? fitnessLevel = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Height = height;
        Weight = weight;
        Gender = gender;
        FitnessGoal = fitnessGoal;
        FitnessLevel = fitnessLevel;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Change user password
    /// </summary>
    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivate user (soft delete)
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate user
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
