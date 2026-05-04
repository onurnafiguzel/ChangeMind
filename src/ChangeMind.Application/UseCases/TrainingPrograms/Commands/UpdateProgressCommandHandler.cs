namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class UpdateProgressCommandHandler(
    ITrainingProgramRepository trainingProgramRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<UpdateProgressCommand>
{
    public async Task Handle(UpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var program = await trainingProgramRepository.GetByIdAsync(request.ProgramId)
            ?? throw new NotFoundException($"Training program with ID '{request.ProgramId}' not found.");

        program.UpdateProgress(request.CompletedWeeks);

        await trainingProgramRepository.UpdateAsync(program);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await Task.WhenAll(
            cache.RemoveAsync(CacheKeys.TrainingProgram(program.Id), cancellationToken),
            cache.RemoveAsync(CacheKeys.UserActiveProgram(program.UserId), cancellationToken));
    }
}
