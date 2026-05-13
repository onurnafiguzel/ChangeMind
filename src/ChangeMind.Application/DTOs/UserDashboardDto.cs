namespace ChangeMind.Application.DTOs;

public class UserDashboardDto
{
    public UserDashboardProfileDto Profile { get; set; } = new();
    public ActiveProgramDetailDto? ActiveTrainingProgram { get; set; }
    public NutritionPlanDetailDto? ActiveNutritionPlan { get; set; }
    public WaitingUserStatusDto? WaitingStatus { get; set; }
}

public class UserDashboardProfileDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? Age { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public string? Gender { get; set; }
    public string? FitnessLevel { get; set; }
    public bool IsCompletedProfile { get; set; }
}
