namespace ChangeMind.Application.Configuration;

public static class CacheKeys
{
    public static string User(Guid id)                  => $"user:{id}";
    public static string Coach(Guid id)                 => $"coach:{id}";
    public static string Package(Guid id)               => $"package:{id}";
    public static string PackageList(bool? isActiveOnly, int page, int pageSize)
                                                        => $"packages:list:{isActiveOnly}:{page}:{pageSize}";
    public static string PackageListPattern()           => "packages:list:*";
    public static string Exercise(Guid id)              => $"exercise:{id}";
    public static string ExerciseList(string? muscleGroup, string? difficultyLevel, string? search, string? sortBy, bool? isActiveOnly, int page, int pageSize)
                                                        => $"exercises:list:{muscleGroup}:{difficultyLevel}:{search}:{sortBy}:{isActiveOnly}:{page}:{pageSize}";
    public static string ExerciseListPattern()          => "exercises:list:*";
    public static string MuscleGroups()                 => "exercises:muscle-groups";
    public static string TrainingProgram(Guid id)       => $"trainingprogram:{id}";
    public static string UserActiveProgram(Guid userId) => $"activeprogram:user:{userId}";
}
