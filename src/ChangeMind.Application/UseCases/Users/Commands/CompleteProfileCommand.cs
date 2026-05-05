namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Domain.Enums; // For Gender and DifficultyLevel

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
    Guid? FitnessGoal = null,
    DifficultyLevel? FitnessLevel = null) : IRequest;
