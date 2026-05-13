namespace ChangeMind.Infrastructure.Data.Configurations;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ToTable("Foods");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedNever();

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Unit)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(FoodMeasurementUnit.Grams);

        builder.Property(f => f.CaloriesPer100g).IsRequired(false).HasPrecision(8, 2);
        builder.Property(f => f.ProteinPer100g).IsRequired(false).HasPrecision(8, 2);
        builder.Property(f => f.CarbsPer100g).IsRequired(false).HasPrecision(8, 2);
        builder.Property(f => f.FatPer100g).IsRequired(false).HasPrecision(8, 2);

        builder.Property(f => f.CaloriesPerPiece).IsRequired(false).HasPrecision(8, 2);
        builder.Property(f => f.ProteinPerPiece).IsRequired(false).HasPrecision(8, 2);
        builder.Property(f => f.CarbsPerPiece).IsRequired(false).HasPrecision(8, 2);
        builder.Property(f => f.FatPerPiece).IsRequired(false).HasPrecision(8, 2);

        builder.Property(f => f.PieceLabel).HasMaxLength(100);
        builder.Property(f => f.GramsPerPiece).IsRequired(false).HasPrecision(8, 2);

        builder.Property(f => f.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.UpdatedAt);

        builder.HasIndex(f => f.Name).IsUnique().HasDatabaseName("IX_Foods_Name");
        builder.HasIndex(f => f.IsActive).HasDatabaseName("IX_Foods_IsActive");
        builder.HasIndex(f => f.Unit).HasDatabaseName("IX_Foods_Unit");
    }
}
