namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        // Table
        builder.ToTable("Coaches");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Specialization)
            .IsRequired(false)
            .HasConversion<string>();

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Coaches_Email");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Coaches_IsActive");

        // Relationships
        builder.HasMany(c => c.AssignedUsers)
            .WithOne(cu => cu.Coach)
            .HasForeignKey(cu => cu.CoachId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CreatedPrograms)
            .WithOne(tp => tp.CreatedBy)
            .HasForeignKey(tp => tp.CoachId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
