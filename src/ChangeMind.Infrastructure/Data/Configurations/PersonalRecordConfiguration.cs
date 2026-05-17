namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PersonalRecordConfiguration : IEntityTypeConfiguration<PersonalRecord>
{
    public void Configure(EntityTypeBuilder<PersonalRecord> builder)
    {
        builder.ToTable("PersonalRecords");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.Lift)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);
        builder.Property(p => p.WeightKg).HasColumnType("numeric(6,2)").IsRequired();
        builder.Property(p => p.RecordedAt).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(p => p.Notes).HasMaxLength(200);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.UserId, p.Lift })
            .HasDatabaseName("IX_PersonalRecords_UserId_Lift");

        builder.Ignore(p => p.DomainEvents);
    }
}
