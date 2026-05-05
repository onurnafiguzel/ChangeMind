namespace ChangeMind.Application.UseCases.FitnessGoals.Commands;

using MediatR;

public record DeleteFitnessGoalCommand(Guid Id) : IRequest;
