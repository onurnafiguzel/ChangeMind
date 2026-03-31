namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? Age { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public Gender? Gender { get; set; }
    public FitnessGoal? FitnessGoal { get; set; }
    public DifficultyLevel? FitnessLevel { get; set; }
    public DateTime CreatedAt { get; set; }
}
