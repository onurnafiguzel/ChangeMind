namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class CreateSelfTrainingProgramCommandHandler(
    ITrainingProgramRepository trainingProgramRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSelfTrainingProgramCommand, Guid>
{
    public async Task<Guid> Handle(CreateSelfTrainingProgramCommand request, CancellationToken cancellationToken)
    {
        _ = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        // Single Self rule: deactivate any existing active Self program for this user.
        var activePrograms = await trainingProgramRepository.GetActiveListByUserIdAsync(request.UserId);
        foreach (var existing in activePrograms.Where(p => p.CreatedByType == CreatedByType.Self))
        {
            existing.Deactivate();
            await trainingProgramRepository.UpdateAsync(existing);
        }

        var trainingProgram = TrainingProgram.CreateBySelf(
            name:          request.Name,
            description:   request.Description,
            durationWeeks: request.DurationWeeks,
            difficulty:    request.Difficulty,
            userId:        request.UserId,
            startDate:     request.StartDate,
            endDate:       request.EndDate);

        if (request.ExercisesByDay is { Count: > 0 })
        {
            var exercisesData = request.ExercisesByDay
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Select(e => new
                    {
                        e.ExerciseId,
                        e.Sets,
                        e.Reps,
                        e.Explanation
                    }).ToList());

            var dailyProgramJson = JsonSerializer.Serialize(exercisesData);
            trainingProgram.UpdateDailyProgram(dailyProgramJson);
        }

        await trainingProgramRepository.AddAsync(trainingProgram);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return trainingProgram.Id;
    }
}
