namespace ChangeMind.Application.UseCases.Exercises.Queries;

using MediatR;

public record GetMuscleGroupsQuery : IRequest<IReadOnlyList<string>>;
