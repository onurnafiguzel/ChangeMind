namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public sealed class UserProfile
{
    private UserProfile() { }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    // Demographic
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public int? Age { get; private set; }
    public decimal? Height { get; private set; }
    public decimal? Weight { get; private set; }
    public Gender? Gender { get; private set; }

    // Fitness
    public Guid? FitnessGoalId { get; private set; }
    public DifficultyLevel? FitnessLevel { get; private set; }

    // Health & lifestyle (free-text, nullable)
    public string? DailyWorkLifestyle { get; private set; }
    public int? GymDaysPerWeek { get; private set; }
    public string? HealthConditions { get; private set; }
    public string? FoodAllergies { get; private set; }
    public string? SupplementInterest { get; private set; }
    public bool WantsSupplementSupport { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public User User { get; private set; } = null!;
    public FitnessGoalItem? FitnessGoal { get; private set; }

    public static UserProfile Create(
        Guid userId,
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
        bool wantsSupplementSupport = false)
    {
        ValidateGymDays(gymDaysPerWeek);

        return new UserProfile
        {
            Id                     = Guid.NewGuid(),
            UserId                 = userId,
            FirstName              = firstName ?? string.Empty,
            LastName               = lastName ?? string.Empty,
            Age                    = age,
            Height                 = height,
            Weight                 = weight,
            Gender                 = gender,
            FitnessGoalId          = fitnessGoalId,
            FitnessLevel           = fitnessLevel,
            DailyWorkLifestyle     = dailyWorkLifestyle,
            GymDaysPerWeek         = gymDaysPerWeek,
            HealthConditions       = healthConditions,
            FoodAllergies          = foodAllergies,
            SupplementInterest     = supplementInterest,
            WantsSupplementSupport = wantsSupplementSupport,
            CreatedAt              = DateTime.UtcNow
        };
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
        ValidateGymDays(gymDaysPerWeek);

        FirstName              = firstName ?? string.Empty;
        LastName               = lastName ?? string.Empty;
        Age                    = age;
        Height                 = height;
        Weight                 = weight;
        Gender                 = gender;
        FitnessGoalId          = fitnessGoalId;
        FitnessLevel           = fitnessLevel;
        DailyWorkLifestyle     = dailyWorkLifestyle;
        GymDaysPerWeek         = gymDaysPerWeek;
        HealthConditions       = healthConditions;
        FoodAllergies          = foodAllergies;
        SupplementInterest     = supplementInterest;
        if (wantsSupplementSupport.HasValue)
            WantsSupplementSupport = wantsSupplementSupport.Value;
        UpdatedAt              = DateTime.UtcNow;
    }

    private static void ValidateGymDays(int? gymDaysPerWeek)
    {
        if (gymDaysPerWeek is < 0 or > 7)
            throw new ValidationException("Gym days per week must be between 0 and 7.");
    }
}
