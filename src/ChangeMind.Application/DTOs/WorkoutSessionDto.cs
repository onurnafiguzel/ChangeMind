namespace ChangeMind.Application.DTOs;

public class WorkoutSessionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? TrainingProgramId { get; set; }
    public string DayKey { get; set; } = string.Empty;
    public DateOnly RecordDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WorkoutExerciseDto> Exercises { get; set; } = new();
}

public class WorkoutExerciseDto
{
    public Guid ExerciseId { get; set; }
    public List<WorkoutSetDto> Sets { get; set; } = new();
}

public class WorkoutSetDto
{
    public int SetNumber { get; set; }
    public decimal Weight { get; set; }
    public int Reps { get; set; }
}
