namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class ActiveProgramDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationWeeks { get; set; }
    public CreatedByType CreatedByType { get; set; }
    public Guid? CoachId { get; set; }
    public string? CoachName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DifficultyLevel? Difficulty { get; set; }
    public string Status { get; set; } = string.Empty; // "InProgress" or "Completed"
    public Dictionary<string, List<ProgramExerciseDetail>>? DailyExercises { get; set; }

    // Assigned user details (JOIN'den)
    public Guid UserId { get; set; }
    public int? UserAge { get; set; }
    public decimal? UserHeight { get; set; }
    public decimal? UserWeight { get; set; }
    public Gender? UserGender { get; set; }
}

public class ProgramExerciseDetail
{
    public Guid ExerciseId { get; set; }
    public int Sets { get; set; }
    public string Reps { get; set; } = string.Empty;
    public string? Explanation { get; set; }
}
