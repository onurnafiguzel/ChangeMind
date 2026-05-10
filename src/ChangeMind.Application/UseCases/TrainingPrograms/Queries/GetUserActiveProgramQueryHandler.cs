namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using Microsoft.Extensions.Options;

public class GetUserActiveProgramQueryHandler(
    ITrainingProgramRepository trainingProgramRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions)
    : IRequestHandler<GetUserActiveProgramQuery, ActiveProgramDetailDto?>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<ActiveProgramDetailDto?> Handle(GetUserActiveProgramQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.UserActiveProgram(request.UserId);

        var cached = await cache.GetAsync<ActiveProgramDetailDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var program = await trainingProgramRepository.GetActiveByUserIdAsync(request.UserId);

        if (program == null)
            return null;

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
            DailyExercises = new Dictionary<string, List<ProgramExerciseDetail>>(),

            UserId     = program.UserId,
            UserAge    = program.AssignedTo.Age,
            UserHeight = program.AssignedTo.Height,
            UserWeight = program.AssignedTo.Weight,
            UserGender = program.AssignedTo.Gender
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
                // Initial boş dict korunur
            }
        }

        await cache.SetAsync(key, dto, TimeSpan.FromSeconds(_cacheOptions.TrainingProgramTtlSeconds), cancellationToken);

        return dto;
    }

    private static string DetermineStatus(DateTime? endDate, DateTime today)
    {
        if (endDate == null || endDate.Value.Date >= today)
            return "InProgress";

        return "Completed";
    }
}
