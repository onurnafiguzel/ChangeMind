namespace ChangeMind.Domain.Entities;

public sealed class WaitingUser
{
    // EF Core constructor — object creation only through factory method
    private WaitingUser() { }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public bool IsWaitingForAssignment { get; private set; } = true;

    public bool HasTrainingProgram { get; private set; }
    public bool HasNutritionPlan { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    public User User { get; private set; } = null!;

    public static WaitingUser Create(Guid userId)
    {
        return new WaitingUser
        {
            Id                     = Guid.NewGuid(),
            UserId                 = userId,
            IsWaitingForAssignment = true,
            HasTrainingProgram     = false,
            HasNutritionPlan       = false,
            CreatedAt              = DateTime.UtcNow,
            UpdatedAt              = null
        };
    }

    /// <summary>
    /// Marks the user as having an active training program. Updates aggregate waiting flag.
    /// </summary>
    public void MarkTrainingProgramAssigned()
    {
        HasTrainingProgram = true;
        RecomputeWaiting();
    }

    /// <summary>
    /// Marks the user as having an active nutrition plan. Updates aggregate waiting flag.
    /// </summary>
    public void MarkNutritionPlanAssigned()
    {
        HasNutritionPlan = true;
        RecomputeWaiting();
    }

    /// <summary>
    /// Clears the training program flag (e.g. on program cancellation).
    /// </summary>
    public void MarkTrainingProgramUnassigned()
    {
        HasTrainingProgram = false;
        RecomputeWaiting();
    }

    /// <summary>
    /// Clears the nutrition plan flag (e.g. on plan cancellation).
    /// </summary>
    public void MarkNutritionPlanUnassigned()
    {
        HasNutritionPlan = false;
        RecomputeWaiting();
    }

    /// <summary>
    /// Legacy convenience: marks both training and nutrition as assigned in one call.
    /// </summary>
    public void MarkAsAssigned()
    {
        HasTrainingProgram = true;
        HasNutritionPlan   = true;
        RecomputeWaiting();
    }

    /// <summary>
    /// Re-queues the user for both training and nutrition assignment (e.g. on re-payment after a cycle).
    /// </summary>
    public void MarkAsWaiting()
    {
        HasTrainingProgram = false;
        HasNutritionPlan   = false;
        RecomputeWaiting();
    }

    private void RecomputeWaiting()
    {
        IsWaitingForAssignment = !(HasTrainingProgram && HasNutritionPlan);
        UpdatedAt              = DateTime.UtcNow;
    }
}
