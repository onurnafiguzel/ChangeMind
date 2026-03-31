namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class Exercise
{
    // Identifier
    public Guid Id { get; set; }

    // Primitive Properties
    public string Name { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    /// <summary>
    ///    Set sayısı. Örneğin: 3 set, 4 set gibi.
    /// </summary>
    public int Sets { get; set; }
    /// <summary>
    /// Reps: "6-8" or "8-12" format
    /// </summary>
    public string? Reps { get; set; }
    /// <summary>
    /// Saniye cinsinden
    /// </summary>
    public int? RestTimeSeconds { get; set; } = 60;
    public string? Notes { get; set; }
    /// <summary>
    /// JSON: Superset, Dropset bilgileri
    /// </summary>
    public string? TechniquesJson { get; set; }

    // Foreign Keys
    public Guid ProgramId { get; set; }

    // Navigation Properties
    public TrainingProgram Program { get; set; }
}
