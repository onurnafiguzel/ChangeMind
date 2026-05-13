namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class CoachDashboardDto
{
    public CoachSummaryDto Coach { get; set; } = new();
    public int AssignedUserCount { get; set; }
    public int ActiveProgramCount { get; set; }
    public int PendingWaitingUserCount { get; set; }
    public List<CoachProgramListItemDto> RecentPrograms { get; set; } = new();
    public List<CoachDashboardAssignedUserDto> AssignedUsers { get; set; } = new();
}

public class CoachSummaryDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public CoachSpecialization? Specialization { get; set; }
}

public class CoachDashboardAssignedUserDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool HasTrainingProgram { get; set; }
    public bool HasNutritionPlan { get; set; }
}
