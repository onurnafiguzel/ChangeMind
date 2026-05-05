namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class Exercise
{
    public Guid Id { get; private set; }
    public string MovementName { get; private set; } = string.Empty;
    public MuscleGroup MuscleGroup { get; private set; }
    public DifficultyLevel? DifficultyLevel { get; private set; }
    public string? Description { get; private set; }
    public string? VideoLink { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF Core constructor
    private Exercise() { }

    public static Exercise Create(
        string movementName,
        MuscleGroup muscleGroup,
        DifficultyLevel difficultyLevel,
        string? description = null,
        string? videoLink = null)
    {
        return new Exercise
        {
            Id              = Guid.NewGuid(),
            MovementName    = movementName,
            MuscleGroup     = muscleGroup,
            DifficultyLevel = difficultyLevel,
            Description     = description,
            VideoLink       = videoLink,
            IsActive        = true,
            CreatedAt       = DateTime.UtcNow
        };
    }

    public void Update(
        string movementName,
        MuscleGroup muscleGroup,
        DifficultyLevel difficultyLevel,
        string? description,
        string? videoLink)
    {
        MovementName    = movementName;
        MuscleGroup     = muscleGroup;
        DifficultyLevel = difficultyLevel;
        Description     = description;
        VideoLink       = videoLink;
        UpdatedAt       = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;
    public void Activate()   => IsActive = true;

    /// <summary>
    /// For EF Core HasData seeding only — do not use in business logic.
    /// </summary>
    public static Exercise Seed(Guid id, string movementName, MuscleGroup muscleGroup, string? description = null)
    {
        return new Exercise
        {
            Id           = id,
            MovementName = movementName,
            MuscleGroup  = muscleGroup,
            Description  = description,
            IsActive     = true,
            CreatedAt    = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
    }
}
