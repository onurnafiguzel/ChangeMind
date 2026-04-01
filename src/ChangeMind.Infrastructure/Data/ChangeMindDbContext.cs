namespace ChangeMind.Infrastructure.Data;

using ChangeMind.Domain.Entities;
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
    }
}
