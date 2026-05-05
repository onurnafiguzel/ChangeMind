namespace ChangeMind.Domain.Entities;

public sealed class FitnessGoalItem
{
    private FitnessGoalItem() { }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static FitnessGoalItem Create(string name, string? description)
    {
        return new FitnessGoalItem
        {
            Id          = Guid.NewGuid(),
            Name        = name,
            Description = description,
            IsActive    = true,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = null
        };
    }

    public void Update(string name, string? description)
    {
        Name        = name;
        Description = description;
        UpdatedAt   = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive  = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
