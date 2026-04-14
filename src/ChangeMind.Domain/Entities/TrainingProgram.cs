namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public sealed class TrainingProgram
{
    // EF Core constructor — object creation only through factory method
    private TrainingProgram() { }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    /// <summary>Hafta cinsinden süre</summary>
    public int DurationWeeks { get; private set; }
    public DifficultyLevel? Difficulty { get; private set; }
    /// <summary>Program versiyonu — her UpdateDailyProgram çağrısında artar</summary>
    public int VersionNumber { get; private set; } = 1;
    public bool IsActive { get; private set; } = true;

    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Guid CoachId { get; private set; }
    public Guid UserId { get; private set; }

    /// <summary>Günlük egzersiz planı (JSON) — UpdateDailyProgram ile güncellenir</summary>
    public string? DailyProgramJson { get; private set; }

    // Navigation Properties
    public Coach CreatedBy { get; private set; } = null!;
    public User AssignedTo { get; private set; } = null!;

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
            Id            = Guid.NewGuid(),
            Name          = name,
            Description   = description,
            DurationWeeks = durationWeeks,
            Difficulty    = difficulty,
            IsActive      = true,
            VersionNumber = 1,
            StartDate     = startDate ?? now,
            EndDate       = endDate   ?? now.AddDays(durationWeeks * 7),
            CoachId       = coachId,
            UserId        = userId,
            CreatedAt     = now,
            UpdatedAt     = null
        };
    }

    /// <summary>
    /// Updates the weekly exercise schedule. Increments VersionNumber on each call.
    /// </summary>
    public void UpdateDailyProgram(string dailyProgramJson)
    {
        DailyProgramJson = dailyProgramJson;
        VersionNumber++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive  = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive  = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
