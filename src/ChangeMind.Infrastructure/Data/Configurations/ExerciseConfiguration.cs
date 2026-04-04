namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        // Table (Exercise Library)
        builder.ToTable("Exercises");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.MuscleGroup)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.MovementName)
            .IsRequired()
            .HasMaxLength(255);

        // Indexes
        builder.HasIndex(e => e.MuscleGroup)
            .HasDatabaseName("IX_Exercises_MuscleGroup");

        builder.HasIndex(e => e.MovementName)
            .HasDatabaseName("IX_Exercises_MovementName");
    }
}
