namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class CompleteTrainingProgramCommandHandler(
    ITrainingProgramRepository trainingProgramRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CompleteTrainingProgramCommand>
{
    public async Task Handle(CompleteTrainingProgramCommand request, CancellationToken cancellationToken)
    {
        var program = await trainingProgramRepository.GetByIdAsync(request.ProgramId)
            ?? throw new NotFoundException($"Training program with ID '{request.ProgramId}' not found.");

        program.Complete();

        await trainingProgramRepository.UpdateAsync(program);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
