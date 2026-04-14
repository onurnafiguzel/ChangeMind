namespace ChangeMind.Infrastructure.Data;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

public class ChangeMindDbContext : DbContext
{
    public ChangeMindDbContext(DbContextOptions<ChangeMindDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<CoachUser> CoachUsers { get; set; }
    public DbSet<TrainingProgram> TrainingPrograms { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<UserPhoto> UserPhotos { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<WaitingUser> WaitingUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CoachConfiguration());
        modelBuilder.ApplyConfiguration(new CoachUserConfiguration());
        modelBuilder.ApplyConfiguration(new TrainingProgramConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new UserPhotoConfiguration());
        modelBuilder.ApplyConfiguration(new PackageConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new WaitingUserConfiguration());

        // Seed Exercise Library
        SeedExerciseLibrary(modelBuilder);
    }

    private static void SeedExerciseLibrary(ModelBuilder modelBuilder)
    {
        // Static GUIDs for exercises (prevents dynamic value warnings in migrations)
        var exercises = new List<Exercise>
        {
            // Chest (IDs: 00000000-0000-0000-0000-000000010001 to 000000010006)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010001"), MuscleGroup.Chest, "Barbell Bench Press"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010002"), MuscleGroup.Chest, "Incline Dumbbell Press"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010003"), MuscleGroup.Chest, "Decline Bench Press"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010004"), MuscleGroup.Chest, "Cable Flyes"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010005"), MuscleGroup.Chest, "Machine Chest Press"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010006"), MuscleGroup.Chest, "Smith Machine Press"),

            // Back (IDs: 00000000-0000-0000-0000-000000020001 to 000000020006)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020001"), MuscleGroup.Back, "Pull-ups"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020002"), MuscleGroup.Back, "Lat Pulldown"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020003"), MuscleGroup.Back, "Bent-over Barbell Row"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020004"), MuscleGroup.Back, "T-Bar Row"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020005"), MuscleGroup.Back, "Seal Row"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020006"), MuscleGroup.Back, "Chest-Supported Row"),

            // Shoulders (IDs: 00000000-0000-0000-0000-000000030001 to 000000030005)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030001"), MuscleGroup.Shoulders, "Overhead Press"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030002"), MuscleGroup.Shoulders, "Lateral Raise"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030003"), MuscleGroup.Shoulders, "Machine Shoulder Press"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030004"), MuscleGroup.Shoulders, "Reverse Pec Deck"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030005"), MuscleGroup.Shoulders, "Arnold Press"),

            // Biceps (IDs: 00000000-0000-0000-0000-000000040001 to 000000040004)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040001"), MuscleGroup.Biceps, "Barbell Curl"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040002"), MuscleGroup.Biceps, "Dumbbell Curl"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040003"), MuscleGroup.Biceps, "Cable Curl"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040004"), MuscleGroup.Biceps, "Preacher Curl"),

            // Triceps (IDs: 00000000-0000-0000-0000-000000050001 to 000000050004)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050001"), MuscleGroup.Triceps, "Tricep Dips"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050002"), MuscleGroup.Triceps, "Rope Pushdown"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050003"), MuscleGroup.Triceps, "Skull Crusher"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050004"), MuscleGroup.Triceps, "Overhead Extension"),

            // Forearms (IDs: 00000000-0000-0000-0000-000000060001 to 000000060002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000060001"), MuscleGroup.Forearms, "Barbell Curl (Reverse)"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000060002"), MuscleGroup.Forearms, "Wrist Curls"),

            // Legs (IDs: 00000000-0000-0000-0000-000000070001 to 000000070007)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070001"), MuscleGroup.Legs, "Barbell Back Squat"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070002"), MuscleGroup.Legs, "Leg Press"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070003"), MuscleGroup.Legs, "Leg Extension"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070004"), MuscleGroup.Legs, "Hamstring Curl"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070005"), MuscleGroup.Legs, "Smith Machine Squat"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070006"), MuscleGroup.Legs, "Hack Squat"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070007"), MuscleGroup.Legs, "Leg Sled"),

            // Quadriceps (IDs: 00000000-0000-0000-0000-000000080001 to 000000080002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000080001"), MuscleGroup.Quadriceps, "Front Squat"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000080002"), MuscleGroup.Quadriceps, "Sissy Squat"),

            // Hamstrings (IDs: 00000000-0000-0000-0000-000000090001 to 000000090002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000090001"), MuscleGroup.Hamstrings, "Romanian Deadlift"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000090002"), MuscleGroup.Hamstrings, "Good Morning"),

            // Glutes (IDs: 00000000-0000-0000-0000-000000100001 to 000000100003)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000100001"), MuscleGroup.Glutes, "Hip Thrust"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000100002"), MuscleGroup.Glutes, "Bulgarian Split Squat"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000100003"), MuscleGroup.Glutes, "Glute-Focused Leg Press"),

            // Calves (IDs: 00000000-0000-0000-0000-000000110001 to 000000110002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000110001"), MuscleGroup.Calves, "Calf Raise"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000110002"), MuscleGroup.Calves, "Machine Calf Raise"),

            // Abs (IDs: 00000000-0000-0000-0000-000000120001 to 000000120003)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000120001"), MuscleGroup.Abs, "Cable Crunch"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000120002"), MuscleGroup.Abs, "Machine Crunch"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000120003"), MuscleGroup.Abs, "Ab Wheel Rollout"),

            // Obliques (IDs: 00000000-0000-0000-0000-000000130001 to 000000130002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000130001"), MuscleGroup.Obliques, "Cable Woodchop"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000130002"), MuscleGroup.Obliques, "Landmine Rotation"),

            // Lower Back (IDs: 00000000-0000-0000-0000-000000140001 to 000000140002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000140001"), MuscleGroup.LowerBack, "Deadlift"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000140002"), MuscleGroup.LowerBack, "Hyperextension"),

            // Traps (IDs: 00000000-0000-0000-0000-000000150001 to 000000150002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000150001"), MuscleGroup.Traps, "Barbell Shrugs"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000150002"), MuscleGroup.Traps, "Dumbbell Shrugs"),

            // Latissimus Dorsi (IDs: 00000000-0000-0000-0000-000000160001 to 000000160002)
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000160001"), MuscleGroup.LatissimusDorsi, "Wide Grip Lat Pulldown"),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000160002"), MuscleGroup.LatissimusDorsi, "Underhand Lat Pulldown")
        };

        modelBuilder.Entity<Exercise>().HasData(exercises);
    }

    private static Exercise CreateExerciseForSeed(Guid id, MuscleGroup muscleGroup, string name)
        => Exercise.Seed(id, name, muscleGroup);
}
