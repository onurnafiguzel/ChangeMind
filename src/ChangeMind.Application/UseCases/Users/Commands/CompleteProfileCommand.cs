namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

/// <summary>
/// Complete user profile with personal and fitness information.
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
    FitnessGoal? FitnessGoal = null,
    DifficultyLevel? FitnessLevel = null) : IRequest;
