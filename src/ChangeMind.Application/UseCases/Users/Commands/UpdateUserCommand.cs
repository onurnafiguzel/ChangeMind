namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    int? Age = null,
    decimal? Height = null,
    decimal? Weight = null,
    Gender? Gender = null,
    FitnessGoal? FitnessGoal = null,
    DifficultyLevel? FitnessLevel = null) : IRequest;
