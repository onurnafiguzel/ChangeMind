namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class TrainingProgram
{
    private TrainingProgram() { }

    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public string Name { get; set; } = string.Empty;
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
    public Coach CreatedBy { get; set; } = null!;
    public User AssignedTo { get; set; } = null!;

    public static TrainingProgram Create(
        string name,
        string? description,
        int durationWeeks,
        DifficultyLevel? difficulty,
        Guid coachId,
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var now = DateTime.UtcNow;
        return new TrainingProgram
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            DurationWeeks = durationWeeks,
            Difficulty = difficulty,
            IsActive = true,
            VersionNumber = 1,
            StartDate = startDate ?? now,
            EndDate = endDate ?? now.AddDays(durationWeeks * 7),
            CoachId = coachId,
            UserId = userId,
            CreatedAt = now,
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Update the daily program JSON (weekly schedule with exercises)
    /// </summary>
    public void UpdateDailyProgram(string dailyProgramJson)
    {
        DailyProgramJson = dailyProgramJson;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
