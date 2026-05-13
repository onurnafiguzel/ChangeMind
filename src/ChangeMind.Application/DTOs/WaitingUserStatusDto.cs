namespace ChangeMind.Application.DTOs;

public class WaitingUserStatusDto
{
    public bool IsWaitingForAssignment { get; set; }
    public bool HasTrainingProgram { get; set; }
    public bool HasNutritionPlan { get; set; }
    public DateTime CreatedAt { get; set; }
}
