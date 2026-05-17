namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;

public class GetUserActiveProgramQueryHandler(
    ITrainingProgramRepository trainingProgramRepository)
    : IRequestHandler<GetUserActiveProgramQuery, ActiveProgramDetailDto?>
{
    public async Task<ActiveProgramDetailDto?> Handle(GetUserActiveProgramQuery request, CancellationToken cancellationToken)
    {
        var programs = await trainingProgramRepository.GetActiveListByUserIdAsync(request.UserId);
        if (programs.Count == 0)
            return null;

        // Coach-assigned takes precedence; fall back to Self if none.
        var selected =
            programs.FirstOrDefault(p => p.CreatedByType == CreatedByType.Coach)
            ?? programs.FirstOrDefault(p => p.CreatedByType == CreatedByType.Self);

        return selected is null ? null : MapToDto(selected, DateTime.UtcNow.Date);
    }

    private static ActiveProgramDetailDto MapToDto(TrainingProgram program, DateTime today)
    {
        var dto = new ActiveProgramDetailDto
        {
            Id             = program.Id,
            Name           = program.Name,
            Description    = program.Description,
            DurationWeeks  = program.DurationWeeks,
            CreatedByType  = program.CreatedByType,
            CoachId        = program.CoachId,
            CoachName      = program.CreatedBy is null
                ? null
                : $"{program.CreatedBy.FirstName} {program.CreatedBy.LastName}",
            StartDate      = program.StartDate,
            EndDate        = program.EndDate,
            Difficulty     = program.Difficulty,
            Status         = DetermineStatus(program.EndDate, today),
            DailyExercises = new Dictionary<string, List<ProgramExerciseDetail>>(),
            UserId         = program.UserId,
            UserAge        = program.AssignedTo.Profile?.Age,
            UserHeight     = program.AssignedTo.Profile?.Height,
            UserWeight     = program.AssignedTo.Profile?.Weight,
            UserGender     = program.AssignedTo.Profile?.Gender
        };

        if (!string.IsNullOrEmpty(program.DailyProgramJson))
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                dto.DailyExercises = JsonSerializer.Deserialize<Dictionary<string, List<ProgramExerciseDetail>>>(
                    program.DailyProgramJson,
                    options) ?? new Dictionary<string, List<ProgramExerciseDetail>>();
            }
            catch
            {
                // ignored — keep empty
            }
        }

        return dto;
    }

    private static string DetermineStatus(DateTime? endDate, DateTime today)
    {
        if (endDate == null || endDate.Value.Date >= today)
            return "InProgress";
        return "Completed";
    }
}
