namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Exceptions;

public sealed class NutritionPlan
{
    private NutritionPlan() { }

    public Guid Id { get; private set; }
    public Guid CoachId { get; private set; }
    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    /// <summary>JSON: { "WorkoutDay": { "meals": [...] }, "OffDay": { "meals": [...] } }</summary>
    public string DailyPlanJson { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;
    public int VersionNumber { get; private set; } = 1;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation
    public User User { get; private set; } = null!;
    public Coach Coach { get; private set; } = null!;

    public static NutritionPlan Create(
        Guid coachId,
        Guid userId,
        string title,
        string? description,
        string dailyPlanJson)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Beslenme planı başlığı boş olamaz.");
        if (string.IsNullOrWhiteSpace(dailyPlanJson))
            throw new ValidationException("Beslenme planı içeriği boş olamaz.");

        return new NutritionPlan
        {
            Id            = Guid.NewGuid(),
            CoachId       = coachId,
            UserId        = userId,
            Title         = title,
            Description   = description,
            DailyPlanJson = dailyPlanJson,
            IsActive      = true,
            VersionNumber = 1,
            CreatedAt     = DateTime.UtcNow
        };
    }

    public void UpdateDailyPlan(string dailyPlanJson)
    {
        if (string.IsNullOrWhiteSpace(dailyPlanJson))
            throw new ValidationException("Beslenme planı içeriği boş olamaz.");

        DailyPlanJson = dailyPlanJson;
        VersionNumber++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMeta(string title, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Beslenme planı başlığı boş olamaz.");

        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
