namespace ChangeMind.Application.UseCases.Exercises.Commands;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class DeleteExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<DeleteExerciseCommand>
{
    public async Task Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await exerciseRepository.GetByIdAsync(request.ExerciseId)
            ?? throw new NotFoundException($"Exercise with ID '{request.ExerciseId}' not found.");

        exercise.Deactivate();

        await exerciseRepository.UpdateAsync(exercise);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(CacheKeys.Exercise(request.ExerciseId), cancellationToken);
    }
}
