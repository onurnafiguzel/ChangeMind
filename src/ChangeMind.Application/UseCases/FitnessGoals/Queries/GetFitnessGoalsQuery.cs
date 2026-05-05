namespace ChangeMind.Application.UseCases.FitnessGoals.Queries;

using ChangeMind.Application.DTOs;
using MediatR;

public record GetFitnessGoalsQuery : IRequest<List<FitnessGoalDto>>;
