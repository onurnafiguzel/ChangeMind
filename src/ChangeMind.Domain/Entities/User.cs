namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Events;

public sealed class User : AggregateRoot
{
    // EF Core constructor — object creation only through factory method
    private User() { }

    public Guid Id { get; private set; }

    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsCompletedProfile { get; private set; } = false;
    public bool IsActive { get; private set; } = true;
    public UserRole Role { get; private set; } = UserRole.User;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation: profile (1:1)
    public UserProfile? Profile { get; private set; }

    // Backward-compat accessors that delegate to Profile (so callers like
    // `user.FirstName` continue to compile and read through the navigation).
    public string FirstName => Profile?.FirstName ?? string.Empty;
    public string LastName  => Profile?.LastName ?? string.Empty;

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
    /// Factory method to create a new User (registration with email and password).
    /// Optional firstName/lastName creates an initial UserProfile row; otherwise profile
    /// remains null until CompleteProfile is called.
    /// </summary>
    public static User Create(
        string email,
        string passwordHash,
        string firstName = "",
        string lastName = "")
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        if (!string.IsNullOrWhiteSpace(firstName) || !string.IsNullOrWhiteSpace(lastName))
        {
            user.Profile = UserProfile.Create(user.Id, firstName, lastName);
        }

        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Email));
        return user;
    }

    /// <summary>
    /// Complete user profile with personal, fitness, and health/lifestyle information.
    /// Upserts the related UserProfile row.
    /// </summary>
    public void CompleteProfile(
        string firstName,
        string lastName,
        int? age = null,
        decimal? height = null,
        decimal? weight = null,
        Gender? gender = null,
        Guid? fitnessGoalId = null,
        DifficultyLevel? fitnessLevel = null,
        string? dailyWorkLifestyle = null,
        int? gymDaysPerWeek = null,
        string? healthConditions = null,
        string? foodAllergies = null,
        string? supplementInterest = null,
        bool? wantsSupplementSupport = null)
    {
        if (Profile is null)
        {
            Profile = UserProfile.Create(
                Id, firstName, lastName, age, height, weight, gender,
                fitnessGoalId, fitnessLevel,
                dailyWorkLifestyle, gymDaysPerWeek, healthConditions, foodAllergies,
                supplementInterest, wantsSupplementSupport ?? false);
        }
        else
        {
            Profile.Update(
                firstName, lastName, age, height, weight, gender,
                fitnessGoalId, fitnessLevel,
                dailyWorkLifestyle, gymDaysPerWeek, healthConditions, foodAllergies,
                supplementInterest, wantsSupplementSupport);
        }

        IsCompletedProfile = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string firstName,
        string lastName,
        int? age = null,
        decimal? height = null,
        decimal? weight = null,
        Gender? gender = null,
        Guid? fitnessGoalId = null,
        DifficultyLevel? fitnessLevel = null,
        string? dailyWorkLifestyle = null,
        int? gymDaysPerWeek = null,
        string? healthConditions = null,
        string? foodAllergies = null,
        string? supplementInterest = null,
        bool? wantsSupplementSupport = null)
    {
        if (Profile is null)
        {
            Profile = UserProfile.Create(
                Id, firstName, lastName, age, height, weight, gender,
                fitnessGoalId, fitnessLevel,
                dailyWorkLifestyle, gymDaysPerWeek, healthConditions, foodAllergies,
                supplementInterest, wantsSupplementSupport ?? false);
        }
        else
        {
            Profile.Update(
                firstName, lastName, age, height, weight, gender,
                fitnessGoalId, fitnessLevel,
                dailyWorkLifestyle, gymDaysPerWeek, healthConditions, foodAllergies,
                supplementInterest, wantsSupplementSupport);
        }

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
        AddDomainEvent(new UserDeactivatedEvent(Id));
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// For DataSeeder only — do not use in business logic.
    /// </summary>
    public static User Seed(
        Guid id,
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role)
    {
        var user = new User
        {
            Id           = id,
            Email        = email,
            PasswordHash = passwordHash,
            Role         = role,
            IsActive     = true,
            CreatedAt    = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        if (!string.IsNullOrWhiteSpace(firstName) || !string.IsNullOrWhiteSpace(lastName))
        {
            user.Profile = UserProfile.Create(id, firstName, lastName);
        }

        return user;
    }
}
