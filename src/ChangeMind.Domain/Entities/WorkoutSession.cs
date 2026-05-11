namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Exceptions;

public sealed class WorkoutSession : AggregateRoot
{
    private WorkoutSession() { }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? TrainingProgramId { get; private set; }
    public string DayKey { get; private set; } = string.Empty;
    public DateOnly RecordDate { get; private set; }
    public string SessionDataJson { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // Navigation
    public User User { get; private set; } = null!;

    public static WorkoutSession Create(
        Guid userId,
        Guid? trainingProgramId,
        string dayKey,
        DateOnly recordDate,
        string sessionDataJson)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("Kullanıcı kimliği geçersiz.");
        if (string.IsNullOrWhiteSpace(dayKey))
            throw new ValidationException("Gün anahtarı boş olamaz.");
        if (string.IsNullOrWhiteSpace(sessionDataJson))
            throw new ValidationException("Seans verisi boş olamaz.");
        if (recordDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ValidationException("Kayıt tarihi gelecekte olamaz.");

        return new WorkoutSession
        {
            Id                = Guid.NewGuid(),
            UserId            = userId,
            TrainingProgramId = trainingProgramId,
            DayKey            = dayKey,
            RecordDate        = recordDate,
            SessionDataJson   = sessionDataJson,
            CreatedAt         = DateTime.UtcNow
        };
    }
}
