namespace ChangeMind.Application.UseCases.FitnessGoals.Commands;

using MediatR;

public record UpdateFitnessGoalCommand(
    Guid Id,
    string Name,
    string? Description = null) : IRequest;
