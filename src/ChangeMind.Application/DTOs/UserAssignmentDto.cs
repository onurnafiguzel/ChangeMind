namespace ChangeMind.Application.DTOs;

public class UserAssignmentDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? Age { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public string? Gender { get; set; }
    public string? FitnessGoal { get; set; }
    public string? FitnessLevel { get; set; }
    public DateTime CreatedAt { get; set; }
}
