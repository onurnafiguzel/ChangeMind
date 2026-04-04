namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;

public class Exercise
{
    // Identifier
    public Guid Id { get; init; }

    // Primitive Properties
    public MuscleGroup MuscleGroup { get; init; }
    public string MovementName { get; init; } = string.Empty;

    // EF Constructor
    public Exercise() { }

    /// <summary>
    /// Factory method to create a new exercise in the library
    /// </summary>
    public static Exercise Create(MuscleGroup muscleGroup, string movementName)
    {
        return new Exercise
        {
            Id = Guid.NewGuid(),
            MuscleGroup = muscleGroup,
            MovementName = movementName
        };
    }
}
