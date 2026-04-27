namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;

public record ActivateTrainingProgramCommand(Guid ProgramId) : IRequest;
