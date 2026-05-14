namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using System.Text.Json;
using ChangeMind.Application.DTOs;

internal static class WorkoutSessionJson
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static List<WorkoutExerciseDto> DeserializeExercises(string sessionDataJson)
    {
        if (string.IsNullOrWhiteSpace(sessionDataJson))
            return new List<WorkoutExerciseDto>();

        try
        {
            var payload = JsonSerializer.Deserialize<SessionPayload>(sessionDataJson, JsonOptions);
            return payload?.Exercises ?? new List<WorkoutExerciseDto>();
        }
        catch
        {
            return new List<WorkoutExerciseDto>();
        }
    }

    private sealed class SessionPayload
    {
        public List<WorkoutExerciseDto> Exercises { get; set; } = new();
    }
}
