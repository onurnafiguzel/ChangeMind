namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FitnessGoalItemConfiguration : IEntityTypeConfiguration<FitnessGoalItem>
{
    public void Configure(EntityTypeBuilder<FitnessGoalItem> builder)
    {
        builder.ToTable("FitnessGoals");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedNever();

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Description)
            .HasMaxLength(1000);

        builder.Property(f => f.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(f => f.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(f => f.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(f => f.Name)
            .IsUnique()
            .HasDatabaseName("UX_FitnessGoals_Name");

        builder.HasIndex(f => f.IsActive)
            .HasDatabaseName("IX_FitnessGoals_IsActive");
    }
}
