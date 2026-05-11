namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.ToTable("WorkoutSessions");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.DayKey).HasMaxLength(20).IsRequired();
        builder.Property(s => s.SessionDataJson).HasColumnType("jsonb").IsRequired();
        builder.Property(s => s.RecordDate).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.UserId, s.DayKey, s.RecordDate })
            .HasDatabaseName("IX_WorkoutSessions_UserId_DayKey_RecordDate");
        builder.HasIndex(s => s.TrainingProgramId);

        builder.Ignore(s => s.DomainEvents);
    }
}
