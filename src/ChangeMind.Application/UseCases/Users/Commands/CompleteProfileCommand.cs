namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

/// <summary>
/// Complete user profile with personal, fitness, and health/lifestyle information.
/// Optionally records initial body measurements and personal records.
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
    bool? WantsSupplementSupport = null,
    // Initial body measurements (optional, cm)
    decimal? WaistCm = null,
    decimal? ArmCm = null,
    decimal? LegCm = null,
    decimal? NeckCm = null,
    decimal? HipCm = null,
    // Initial personal records (optional, kg)
    decimal? BenchPressPR = null,
    decimal? SquatPR = null,
    decimal? DeadliftPR = null,
    decimal? OverheadPressPR = null,
    decimal? BarbellRowPR = null,
    decimal? PullUpPR = null) : IRequest;
