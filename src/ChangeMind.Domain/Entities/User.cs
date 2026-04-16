namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public sealed class User
{
    // EF Core constructor — object creation only through factory method
    private User() { }

    public Guid Id { get; private set; }

    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public int? Age { get; private set; }
    public decimal? Height { get; private set; }
    public decimal? Weight { get; private set; }
    public Gender? Gender { get; private set; }
    public FitnessGoal? FitnessGoal { get; private set; }
    public DifficultyLevel? FitnessLevel { get; private set; }
    public bool IsActive { get; private set; } = true;
    public UserRole Role { get; private set; } = UserRole.User;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    private readonly List<UserPhoto> _photos = new();
    public IReadOnlyCollection<UserPhoto> Photos => _photos.AsReadOnly();

    private readonly List<TrainingProgram> _assignedPrograms = new();
    public IReadOnlyCollection<TrainingProgram> AssignedPrograms => _assignedPrograms.AsReadOnly();

    private readonly List<CoachUser> _coachRelationships = new();
    public IReadOnlyCollection<CoachUser> CoachRelationships => _coachRelationships.AsReadOnly();

    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    public WaitingUser? WaitingUserRecord { get; private set; }

    /// <summary>
    /// Factory method to create a new User (registration with email and password only)
    /// </summary>
    public static User Create(
        string email,
        string passwordHash,
        string firstName = "",
        string lastName = "")
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            Age = null,
            Height = null,
            Weight = null,
            Gender = null,
            FitnessGoal = null,
            FitnessLevel = null,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Complete user profile with personal and fitness information (called after registration)
    /// </summary>
    public void CompleteProfile(
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

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// For DataSeeder only — do not use in business logic.
    /// Allows setting Role and static timestamps required for idempotent seeding.
    /// </summary>
    public static User Seed(
        Guid id,
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role)
    {
        return new User
        {
            Id           = id,
            Email        = email,
            PasswordHash = passwordHash,
            FirstName    = firstName,
            LastName     = lastName,
            Role         = role,
            IsActive     = true,
            CreatedAt    = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
    }
}
