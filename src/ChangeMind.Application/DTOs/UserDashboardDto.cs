namespace ChangeMind.Application.DTOs;

public class UserDashboardDto
{
    public UserDashboardProfileDto Profile { get; set; } = new();
    public ActiveProgramDetailDto? ActiveTrainingProgram { get; set; }
    public NutritionPlanDetailDto? ActiveNutritionPlan { get; set; }
    public WaitingUserStatusDto? WaitingStatus { get; set; }
    public PackageProgressDto? PackageProgress { get; set; }
}

public class PackageProgressDto
{
    public Guid PaymentId { get; set; }
    public Guid PackageId { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public int ElapsedDays { get; set; }
    public int RemainingDays { get; set; }
    /// <summary>0..100, two decimals.</summary>
    public decimal ProgressPercentage { get; set; }
    public bool IsExpired { get; set; }
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
