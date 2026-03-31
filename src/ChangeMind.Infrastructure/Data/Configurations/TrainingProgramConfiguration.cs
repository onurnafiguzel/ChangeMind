namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TrainingProgramConfiguration : IEntityTypeConfiguration<TrainingProgram>
{
    public void Configure(EntityTypeBuilder<TrainingProgram> builder)
    {
        // Table
        builder.ToTable("TrainingPrograms");

        // Primary Key
        builder.HasKey(tp => tp.Id);

        // Properties
        builder.Property(tp => tp.Id)
            .ValueGeneratedNever();

        builder.Property(tp => tp.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(tp => tp.Description)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(tp => tp.CoachId)
            .IsRequired();

        builder.Property(tp => tp.UserId)
            .IsRequired();

        builder.Property(tp => tp.DurationWeeks)
            .IsRequired();

        builder.Property(tp => tp.Difficulty)
            .IsRequired(false)
            .HasConversion<string>();

        builder.Property(tp => tp.VersionNumber)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(tp => tp.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(tp => tp.StartDate)
            .IsRequired(false);

        builder.Property(tp => tp.EndDate)
            .IsRequired(false);

        builder.Property(tp => tp.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(tp => tp.UpdatedAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(tp => tp.CoachId)
            .HasDatabaseName("IX_TrainingPrograms_CoachId");

        builder.HasIndex(tp => tp.UserId)
            .HasDatabaseName("IX_TrainingPrograms_UserId");

        builder.HasIndex(tp => tp.IsActive)
            .HasDatabaseName("IX_TrainingPrograms_IsActive");

        // Relationships
        builder.HasOne(tp => tp.CreatedBy)
            .WithMany(c => c.CreatedPrograms)
            .HasForeignKey(tp => tp.CoachId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tp => tp.AssignedTo)
            .WithMany(u => u.AssignedPrograms)
            .HasForeignKey(tp => tp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(tp => tp.Exercises)
            .WithOne(e => e.Program)
            .HasForeignKey(e => e.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
