namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetTrainingProgramByIdQueryHandler(ITrainingProgramRepository trainingProgramRepository)
    : IRequestHandler<GetTrainingProgramByIdQuery, ActiveProgramDetailDto?>
{
    public async Task<ActiveProgramDetailDto?> Handle(GetTrainingProgramByIdQuery request, CancellationToken cancellationToken)
    {
        var program = await trainingProgramRepository.GetByIdAsync(request.ProgramId);
        if (program == null)
            return null;

        var today = DateTime.UtcNow.Date;
        var status = program.EndDate == null || program.EndDate.Value.Date >= today
            ? "InProgress"
            : "Completed";

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

        if (!string.IsNullOrEmpty(program.DailyProgramJson))
        {
            try
            {
                dto.DailyExercises = JsonSerializer.Deserialize<Dictionary<string, List<ProgramExerciseDetail>>>(
                    program.DailyProgramJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                dto.DailyExercises = null;
            }
        }

        return dto;
    }
}
