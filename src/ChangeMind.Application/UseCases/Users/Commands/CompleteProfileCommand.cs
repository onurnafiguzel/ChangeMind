namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

/// <summary>
/// Complete user profile with personal, fitness, and health/lifestyle information.
/// Called after initial registration (email/password).
/// </summary>
public record CompleteProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    int? Age = null,
    decimal? Height = null,
    decimal? Weight = null,
    Gender? Gender = null,
    Guid? FitnessGoalId = null,
    DifficultyLevel? FitnessLevel = null,
    // Health & lifestyle (optional)
    string? DailyWorkLifestyle = null,
    int? GymDaysPerWeek = null,
    string? HealthConditions = null,
    string? FoodAllergies = null,
    string? SupplementInterest = null,
    bool? WantsSupplementSupport = null) : IRequest;
