namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;

public record CompleteTrainingProgramCommand(Guid ProgramId) : IRequest;
