namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;

public record UpdateProgressCommand(Guid ProgramId, int CompletedWeeks) : IRequest;
