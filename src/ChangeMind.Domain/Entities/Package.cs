namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public sealed class Package
{
    // EF Core constructor — object creation only through factory method
    private Package() { }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int DurationDays { get; private set; }
    public PackageType Type { get; private set; }
    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    public static Package Create(
        string name,
        string description,
        decimal price,
        int durationDays,
        PackageType type)
    {
        return new Package
        {
            Id          = Guid.NewGuid(),
            Name        = name,
            Description = description,
            Price       = price,
            DurationDays = durationDays,
            Type        = type,
            IsActive    = true,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = null
        };
    }

    public void Update(
        string name,
        string description,
        decimal price,
        int durationDays,
        PackageType type)
    {
        Name        = name;
        Description = description;
        Price       = price;
        DurationDays = durationDays;
        Type        = type;
        UpdatedAt   = DateTime.UtcNow;
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
