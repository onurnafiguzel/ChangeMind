namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;

public class CreateTrainingProgramCommandHandler(
    ITrainingProgramRepository trainingProgramRepository,
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    IWaitingUserRepository waitingUserRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTrainingProgramCommand, Guid>
{
    public async Task<Guid> Handle(CreateTrainingProgramCommand request, CancellationToken cancellationToken)
    {
        // Verify coach exists
        var coach = await coachRepository.GetByIdAsync(request.CoachId)
            ?? throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        // Verify user exists
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        // Resolve waiting record — user must be in waiting queue (payment completed)
        var waitingUser = await waitingUserRepository.GetByUserIdAsync(request.UserId)
            ?? throw new NotFoundException(
                $"Waiting record for user '{request.UserId}' not found. Payment may be incomplete.");

        // Duplicate guard: training program already assigned — do not allow another one
        if (waitingUser.HasTrainingProgram)
            throw new ConflictException(
                $"User '{request.UserId}' already has an active training program assigned.");

        // Create training program
        var trainingProgram = TrainingProgram.CreateByCoach(
            name: request.Name,
            description: request.Description,
            durationWeeks: request.DurationWeeks,
            difficulty: request.Difficulty,
            coachId: request.CoachId,
            userId: request.UserId,
            startDate: request.StartDate,
            endDate: request.EndDate);

        // Generate DailyProgramJson from exercises if provided
        if (request.ExercisesByDay != null && request.ExercisesByDay.Count > 0)
        {
            // Convert exercises to serializable format organized by day
            var exercisesData = request.ExercisesByDay
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Select(e => new
                    {
                        e.ExerciseId,
                        e.Sets,
                        e.Reps,
                        e.Explanation
                    }).ToList()
                );

            var dailyProgramJson = JsonSerializer.Serialize(exercisesData);
            trainingProgram.UpdateDailyProgram(dailyProgramJson);
        }

        // Save training program
        await trainingProgramRepository.AddAsync(trainingProgram);

        // Mark waiting record: flips IsWaitingForAssignment=false only if nutrition is also assigned
        waitingUser.MarkTrainingProgramAssigned();
        await waitingUserRepository.UpdateAsync(waitingUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return trainingProgram.Id;
    }
}
