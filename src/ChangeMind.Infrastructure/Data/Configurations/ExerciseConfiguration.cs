namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("Exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.MuscleGroup)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(e => e.DifficultyLevel)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.VideoUrl)
            .HasMaxLength(500);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);

        builder.HasIndex(e => e.MuscleGroup)
            .HasDatabaseName("IX_Exercises_MuscleGroup");

        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_Exercises_Name");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_Exercises_IsActive");
    }
}
