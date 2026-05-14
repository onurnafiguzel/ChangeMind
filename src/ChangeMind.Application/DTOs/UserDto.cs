namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums; // For Gender and DifficultyLevel

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? Age { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public Gender? Gender { get; set; }
    public string FitnessGoal { get; set; } = string.Empty;
    public DifficultyLevel? FitnessLevel { get; set; }
    public bool IsCompletedProfile { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserHealthBlockDto? HealthProfile { get; set; }
}

public class UserHealthBlockDto
{
    public string? DailyWorkLifestyle { get; set; }
    public int? GymDaysPerWeek { get; set; }
    public string? HealthConditions { get; set; }
    public string? FoodAllergies { get; set; }
    public string? SupplementInterest { get; set; }
    public bool WantsSupplementSupport { get; set; }
}
