namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class Coach
{
    // Identifier
    public Guid Id { get; private set; }

    // Primitive Properties
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public CoachSpecialization? Specialization { get; private set; }
    public bool IsActive { get; private set; } = true;

    // DateTime Properties
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    private readonly List<CoachUser> _assignedUsers = new();
    public IReadOnlyCollection<CoachUser> AssignedUsers => _assignedUsers.AsReadOnly();

    private readonly List<TrainingProgram> _createdPrograms = new();
    public IReadOnlyCollection<TrainingProgram> CreatedPrograms => _createdPrograms.AsReadOnly();

    // EF Constructor
    private protected Coach() { }

    /// <summary>
    /// Factory method to create a new Coach
    /// </summary>
    public static Coach Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        CoachSpecialization? specialization = null)
    {
        return new Coach
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            Specialization = specialization,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    /// <summary>
    /// Update coach profile information
    /// </summary>
    public void Update(
        string firstName,
        string lastName,
        CoachSpecialization? specialization = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Specialization = specialization;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Change coach password
    /// </summary>
    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivate coach (soft delete)
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Activate coach
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
