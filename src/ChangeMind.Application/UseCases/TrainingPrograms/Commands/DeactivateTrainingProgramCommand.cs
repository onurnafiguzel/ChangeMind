namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;

public record DeactivateTrainingProgramCommand(Guid ProgramId) : IRequest;
