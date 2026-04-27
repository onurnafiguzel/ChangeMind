namespace ChangeMind.Application.UseCases.TrainingPrograms.Commands;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class UpdateDailyProgramCommandHandler(
    ITrainingProgramRepository trainingProgramRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateDailyProgramCommand>
{
    public async Task Handle(UpdateDailyProgramCommand request, CancellationToken cancellationToken)
    {
        var program = await trainingProgramRepository.GetByIdAsync(request.ProgramId)
            ?? throw new NotFoundException($"Training program with ID '{request.ProgramId}' not found.");

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

        var json = JsonSerializer.Serialize(exercisesData);
        program.UpdateDailyProgram(json);

        await trainingProgramRepository.UpdateAsync(program);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
