namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

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
    public bool IsCompleted { get; private set; } = false;

    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Guid CoachId { get; private set; }
    public Guid UserId { get; private set; }

    /// <summary>Tamamlanan hafta sayısı — progress takibi için</summary>
    public int CompletedWeeks { get; private set; } = 0;

    /// <summary>Günlük egzersiz planı (JSON) — UpdateDailyProgram ile güncellenir</summary>
    public string? DailyProgramJson { get; private set; }

    // Navigation Properties
    public Coach CreatedBy { get; private set; } = null!;
    public User AssignedTo { get; private set; } = null!;

    /// <summary>0-100 arası ilerleme yüzdesi</summary>
    public double ProgressPercentage =>
        DurationWeeks > 0 ? Math.Min(100.0, (CompletedWeeks / (double)DurationWeeks) * 100.0) : 0;

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
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Program adı boş olamaz.");

        if (durationWeeks < 1 || durationWeeks > 52)
            throw new ValidationException("Program süresi 1 ile 52 hafta arasında olmalıdır.");

        var now = DateTime.UtcNow;
        return new TrainingProgram
        {
            Id             = Guid.NewGuid(),
            Name           = name,
            Description    = description,
            DurationWeeks  = durationWeeks,
            Difficulty     = difficulty,
            IsActive       = true,
            IsCompleted    = false,
            VersionNumber  = 1,
            CompletedWeeks = 0,
            StartDate      = startDate ?? now,
            EndDate        = endDate   ?? now.AddDays(durationWeeks * 7),
            CoachId        = coachId,
            UserId         = userId,
            CreatedAt      = now,
            UpdatedAt      = null,
            CompletedAt    = null
        };
    }

    /// <summary>
    /// Günlük egzersiz planını günceller. Her çağrıda VersionNumber artar.
    /// </summary>
    public void UpdateDailyProgram(string dailyProgramJson)
    {
        if (string.IsNullOrWhiteSpace(dailyProgramJson))
            throw new ValidationException("Günlük program içeriği boş olamaz.");

        if (!IsActive)
            throw new InvalidStateTransitionException("TrainingProgram", "inactive", "update daily program");

        if (IsCompleted)
            throw new InvalidStateTransitionException("TrainingProgram", "completed", "update daily program");

        DailyProgramJson = dailyProgramJson;
        VersionNumber++;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Program ilerlemesini günceller (tamamlanan hafta sayısı).
    /// </summary>
    public void UpdateProgress(int completedWeeks)
    {
        if (completedWeeks < 0 || completedWeeks > DurationWeeks)
            throw new ValidationException($"Tamamlanan hafta sayısı 0 ile {DurationWeeks} arasında olmalıdır.");

        if (!IsActive)
            throw new InvalidStateTransitionException("TrainingProgram", "inactive", "update progress");

        if (IsCompleted)
            throw new InvalidStateTransitionException("TrainingProgram", "completed", "update progress");

        CompletedWeeks = completedWeeks;
        UpdatedAt      = DateTime.UtcNow;
    }

    /// <summary>
    /// Programı tamamlandı olarak işaretler.
    /// </summary>
    public void Complete()
    {
        if (!IsActive)
            throw new InvalidStateTransitionException("TrainingProgram", "inactive", "complete");

        if (IsCompleted)
            throw new InvalidStateTransitionException("TrainingProgram", "already completed", "complete");

        IsCompleted    = true;
        IsActive       = false;
        CompletedWeeks = DurationWeeks;
        CompletedAt    = DateTime.UtcNow;
        UpdatedAt      = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidStateTransitionException("TrainingProgram", "inactive", "deactivate");

        IsActive  = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsCompleted)
            throw new InvalidStateTransitionException("TrainingProgram", "completed", "activate");

        IsActive  = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
