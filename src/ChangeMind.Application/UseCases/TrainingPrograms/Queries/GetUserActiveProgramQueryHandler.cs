namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetUserActiveProgramQueryHandler(ITrainingProgramRepository trainingProgramRepository)
    : IRequestHandler<GetUserActiveProgramQuery, ActiveProgramDetailDto?>
{
    public async Task<ActiveProgramDetailDto?> Handle(GetUserActiveProgramQuery request, CancellationToken cancellationToken)
    {
        var program = await trainingProgramRepository.GetActiveByUserIdAsync(request.UserId);

        if (program == null)
        {
            return null;
        }

        var today = DateTime.UtcNow.Date;
        var status = DetermineStatus(program.EndDate, today);

        var dto = new ActiveProgramDetailDto
        {
            Id = program.Id,
            Name = program.Name,
            Description = program.Description,
            DurationWeeks = program.DurationWeeks,
            CoachName = $"{program.CreatedBy.FirstName} {program.CreatedBy.LastName}",
            StartDate = program.StartDate,
            EndDate = program.EndDate,
            Difficulty = program.Difficulty,
            Status = status,
            DailyExercises = null
        };

        // Deserialize daily exercises if available
        if (!string.IsNullOrEmpty(program.DailyProgramJson))
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dailyExercises = JsonSerializer.Deserialize<Dictionary<string, List<ProgramExerciseDetail>>>(
                    program.DailyProgramJson,
                    options);
                dto.DailyExercises = dailyExercises;
            }
            catch
            {
                // If deserialization fails, leave DailyExercises as null
                dto.DailyExercises = null;
            }
        }

        return dto;
    }

    private static string DetermineStatus(DateTime? endDate, DateTime today)
    {
        if (endDate == null || endDate.Value.Date >= today)
        {
            return "InProgress";
        }
        return "Completed";
    }
}
