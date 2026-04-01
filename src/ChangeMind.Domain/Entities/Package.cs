namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class Package
{
    // Identifier
    public Guid Id { get; private set; }

    // Primitive Properties
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int DurationDays { get; private set; }
    public PackageType Type { get; private set; }
    public bool IsActive { get; private set; } = true;

    // DateTime Properties
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF Constructor
    private protected Package() { }

    /// <summary>
    /// Factory method to create a new Package
    /// </summary>
    public static Package Create(
        string name,
        string description,
        decimal price,
        int durationDays,
        PackageType type)
    {
        return new Package
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            DurationDays = durationDays,
            Type = type,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Update package information
    /// </summary>
    public void Update(
        string name,
        string description,
        decimal price,
        int durationDays,
        PackageType type)
    {
        Name = name;
        Description = description;
        Price = price;
        DurationDays = durationDays;
        Type = type;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivate package (soft delete)
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate package
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
