namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CoachUserConfiguration : IEntityTypeConfiguration<CoachUser>
{
    public void Configure(EntityTypeBuilder<CoachUser> builder)
    {
        // Table
        builder.ToTable("CoachUsers");

        // Primary Key
        builder.HasKey(cu => cu.Id);

        // Properties
        builder.Property(cu => cu.Id)
            .ValueGeneratedNever();

        builder.Property(cu => cu.CoachId)
            .IsRequired();

        builder.Property(cu => cu.UserId)
            .IsRequired();

        builder.Property(cu => cu.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(cu => cu.AssignedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(cu => cu.UnassignedAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(cu => new { cu.CoachId, cu.UserId })
            .IsUnique()
            .HasDatabaseName("IX_CoachUsers_CoachId_UserId");

        builder.HasIndex(cu => cu.IsActive)
            .HasDatabaseName("IX_CoachUsers_IsActive");

        // Foreign Keys
        builder.HasOne(cu => cu.Coach)
            .WithMany(c => c.AssignedUsers)
            .HasForeignKey(cu => cu.CoachId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cu => cu.User)
            .WithMany(u => u.CoachRelationships)
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
