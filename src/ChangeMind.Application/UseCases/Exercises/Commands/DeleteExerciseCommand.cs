namespace ChangeMind.Application.UseCases.Exercises.Commands;

using MediatR;

public record DeleteExerciseCommand(Guid ExerciseId) : IRequest;
