namespace ChangeMind.Application.UseCases.Exercises.Commands;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.Extensions;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class UpdateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<UpdateExerciseCommand>
{
    public async Task Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await exerciseRepository.GetByIdAsync(request.ExerciseId)
            ?? throw new NotFoundException($"Exercise with ID '{request.ExerciseId}' not found.");

        if (!string.Equals(exercise.Name, request.Name, StringComparison.OrdinalIgnoreCase)
            && await exerciseRepository.ExistsAsync(request.Name))
            throw new ConflictException($"An exercise named '{request.Name}' already exists.");

        var muscleGroup     = request.MuscleGroup.ParseOrThrow<MuscleGroup>();
        var difficultyLevel = request.DifficultyLevel.ParseOrThrow<DifficultyLevel>();

        exercise.Update(
            name:            request.Name,
            muscleGroup:     muscleGroup,
            difficultyLevel: difficultyLevel,
            description:     request.Description,
            videoUrl:        request.VideoUrl);

        await exerciseRepository.UpdateAsync(exercise);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await Task.WhenAll(
            cache.RemoveAsync(CacheKeys.Exercise(request.ExerciseId), cancellationToken),
            cache.RemoveAsync(CacheKeys.MuscleGroups(), cancellationToken),
            cache.RemoveByPatternAsync(CacheKeys.ExerciseListPattern(), cancellationToken));
    }
}
