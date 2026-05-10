namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class CoachProgramListItemDto
{
    // Program fields
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationWeeks { get; set; }
    public int CompletedWeeks { get; set; }
    public double ProgressPercentage { get; set; }
    public DifficultyLevel? Difficulty { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }

    // User details (JOIN'den)
    public Guid UserId { get; set; }
    public int? UserAge { get; set; }
    public decimal? UserHeight { get; set; }
    public decimal? UserWeight { get; set; }
    public Gender? UserGender { get; set; }
}
