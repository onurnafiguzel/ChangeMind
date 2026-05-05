namespace ChangeMind.Application.UseCases.FitnessGoals.Commands;

using MediatR;

public record CreateFitnessGoalCommand(
    string Name,
    string? Description = null) : IRequest<Guid>;
