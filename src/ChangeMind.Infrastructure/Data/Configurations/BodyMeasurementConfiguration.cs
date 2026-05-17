namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BodyMeasurementConfiguration : IEntityTypeConfiguration<BodyMeasurement>
{
    public void Configure(EntityTypeBuilder<BodyMeasurement> builder)
    {
        builder.ToTable("BodyMeasurements");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.UserId).IsRequired();
        builder.Property(m => m.RecordedAt).IsRequired();
        builder.Property(m => m.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");

        builder.Property(m => m.WeightKg).HasColumnType("numeric(6,2)");
        builder.Property(m => m.WaistCm).HasColumnType("numeric(5,2)");
        builder.Property(m => m.ArmCm).HasColumnType("numeric(5,2)");
        builder.Property(m => m.LegCm).HasColumnType("numeric(5,2)");
        builder.Property(m => m.NeckCm).HasColumnType("numeric(5,2)");
        builder.Property(m => m.HipCm).HasColumnType("numeric(5,2)");
        builder.Property(m => m.BodyFatPercent).HasColumnType("numeric(5,2)");
        builder.Property(m => m.Notes).HasMaxLength(500);

        builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.UserId, m.RecordedAt })
            .IsDescending(false, true)
            .HasDatabaseName("IX_BodyMeasurements_UserId_RecordedAt");

        builder.Ignore(m => m.DomainEvents);
    }
}
