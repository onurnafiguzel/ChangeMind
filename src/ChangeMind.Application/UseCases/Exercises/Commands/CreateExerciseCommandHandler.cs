namespace ChangeMind.Application.UseCases.Exercises.Commands;

using ChangeMind.Application.Extensions;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class CreateExerciseCommandHandler(
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateExerciseCommand, Guid>
{
    public async Task<Guid> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        if (await exerciseRepository.ExistsAsync(request.Name))
            throw new ConflictException($"An exercise named '{request.Name}' already exists.");

        var muscleGroup     = request.MuscleGroup.ParseOrThrow<MuscleGroup>();
        var difficultyLevel = request.DifficultyLevel.ParseOrThrow<DifficultyLevel>();

        var exercise = Exercise.Create(
            name:            request.Name,
            muscleGroup:     muscleGroup,
            difficultyLevel: difficultyLevel,
            description:     request.Description,
            videoUrl:        request.VideoUrl);

        await exerciseRepository.AddAsync(exercise);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exercise.Id;
    }
}
