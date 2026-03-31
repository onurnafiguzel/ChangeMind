namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        // Table
        builder.ToTable("Exercises");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.ProgramId)
            .IsRequired();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.MuscleGroup)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Sets)
            .IsRequired();

        builder.Property(e => e.Reps)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(e => e.RestTimeSeconds)
            .IsRequired(false)
            .HasDefaultValue(60);

        builder.Property(e => e.Notes)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(e => e.TechniquesJson)
            .IsRequired(false)
            .HasColumnType("text");

        // Indexes
        builder.HasIndex(e => e.ProgramId)
            .HasDatabaseName("IX_Exercises_ProgramId");

        builder.HasIndex(e => e.MuscleGroup)
            .HasDatabaseName("IX_Exercises_MuscleGroup");

        // Relationships
        builder.HasOne(e => e.Program)
            .WithMany(tp => tp.Exercises)
            .HasForeignKey(e => e.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
