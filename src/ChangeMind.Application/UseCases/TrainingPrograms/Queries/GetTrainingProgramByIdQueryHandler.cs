namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using Microsoft.Extensions.Options;

public class GetTrainingProgramByIdQueryHandler(
    ITrainingProgramRepository trainingProgramRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions)
    : IRequestHandler<GetTrainingProgramByIdQuery, ActiveProgramDetailDto?>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<ActiveProgramDetailDto?> Handle(GetTrainingProgramByIdQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.TrainingProgram(request.ProgramId);

        var cached = await cache.GetAsync<ActiveProgramDetailDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

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
            DailyExercises = new Dictionary<string, List<ProgramExerciseDetail>>(),

            UserId     = program.UserId,
            UserAge    = program.AssignedTo.Profile?.Age,
            UserHeight = program.AssignedTo.Profile?.Height,
            UserWeight = program.AssignedTo.Profile?.Weight,
            UserGender = program.AssignedTo.Profile?.Gender
        };

        if (!string.IsNullOrEmpty(program.DailyProgramJson))
        {
            try
            {
                dto.DailyExercises = JsonSerializer.Deserialize<Dictionary<string, List<ProgramExerciseDetail>>>(
                    program.DailyProgramJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Dictionary<string, List<ProgramExerciseDetail>>();
            }
            catch
            {
                // Initial boş dict korunur
            }
        }

        await cache.SetAsync(key, dto, TimeSpan.FromSeconds(_cacheOptions.TrainingProgramTtlSeconds), cancellationToken);

        return dto;
    }
}
