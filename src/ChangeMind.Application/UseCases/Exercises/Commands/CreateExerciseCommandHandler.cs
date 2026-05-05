namespace ChangeMind.Application.UseCases.Exercises.Commands;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.Extensions;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class CreateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<CreateExerciseCommand, Guid>
{
    public async Task<Guid> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        if (await exerciseRepository.ExistsAsync(request.Name))
            throw new ConflictException($"An exercise named '{request.Name}' already exists.");

        var muscleGroup     = request.MuscleGroup.ParseOrThrow<MuscleGroup>();
        var difficultyLevel = request.DifficultyLevel.ParseOrThrow<DifficultyLevel>();

        var exercise = Exercise.Create(
            movementName:    request.Name,
            muscleGroup:     muscleGroup,
            difficultyLevel: difficultyLevel,
            description:     request.Description,
            videoLink:       request.VideoUrl);

        await exerciseRepository.AddAsync(exercise);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await Task.WhenAll(
            cache.RemoveAsync(CacheKeys.MuscleGroups(), cancellationToken),
            cache.RemoveByPatternAsync(CacheKeys.ExerciseListPattern(), cancellationToken));

        return exercise.Id;
    }
}
