namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class TrainingProgram
{
    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public string Name { get; set; }
    public string? Description { get; set; }
    /// <summary>
    /// Hafta cinsinden süre
    /// </summary>
    public int DurationWeeks { get; set; }
    public DifficultyLevel? Difficulty { get; set; }
    /// <summary>
    /// Program versiyonu (düzenleme sırasında increments)
    /// </summary>
    public int VersionNumber { get; set; } = 1;
    public bool IsActive { get; set; } = true;

    // DateTime Properties
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public Guid CoachId { get; set; }
    public Guid UserId { get; set; }

    // Primitive Properties - Daily Program Schedule (JSON)
    public string? DailyProgramJson { get; private set; }

    // Navigation Properties
    public Coach CreatedBy { get; set; }
    public User AssignedTo { get; set; }

    /// <summary>
    /// Update the daily program JSON (weekly schedule with exercises)
    /// </summary>
    public void UpdateDailyProgram(string dailyProgramJson)
    {
        DailyProgramJson = dailyProgramJson;
        UpdatedAt = DateTime.UtcNow;
    }
}
