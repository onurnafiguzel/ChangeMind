namespace ChangeMind.Domain.Entities;

using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class Food
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public FoodMeasurementUnit Unit { get; private set; } = FoodMeasurementUnit.Grams;

    // Per 100g (Unit=Grams)
    public decimal? CaloriesPer100g { get; private set; }
    public decimal? ProteinPer100g { get; private set; }
    public decimal? CarbsPer100g { get; private set; }
    public decimal? FatPer100g { get; private set; }

    // Per piece (Unit=Piece)
    public decimal? CaloriesPerPiece { get; private set; }
    public decimal? ProteinPerPiece { get; private set; }
    public decimal? CarbsPerPiece { get; private set; }
    public decimal? FatPerPiece { get; private set; }

    public string? PieceLabel { get; private set; }
    public decimal? GramsPerPiece { get; private set; }

    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Food() { }

    public static Food CreateGrams(
        string name,
        decimal caloriesPer100g,
        decimal proteinPer100g,
        decimal carbsPer100g,
        decimal fatPer100g)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Besin adı boş olamaz.");

        return new Food
        {
            Id              = Guid.NewGuid(),
            Name            = name,
            Unit            = FoodMeasurementUnit.Grams,
            CaloriesPer100g = caloriesPer100g,
            ProteinPer100g  = proteinPer100g,
            CarbsPer100g    = carbsPer100g,
            FatPer100g      = fatPer100g,
            IsActive        = true,
            CreatedAt       = DateTime.UtcNow
        };
    }

    public static Food CreatePiece(
        string name,
        decimal caloriesPerPiece,
        decimal proteinPerPiece,
        decimal carbsPerPiece,
        decimal fatPerPiece,
        string pieceLabel,
        decimal? gramsPerPiece = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Besin adı boş olamaz.");
        if (string.IsNullOrWhiteSpace(pieceLabel))
            throw new ValidationException("Adet etiketi boş olamaz.");

        return new Food
        {
            Id               = Guid.NewGuid(),
            Name             = name,
            Unit             = FoodMeasurementUnit.Piece,
            CaloriesPerPiece = caloriesPerPiece,
            ProteinPerPiece  = proteinPerPiece,
            CarbsPerPiece    = carbsPerPiece,
            FatPerPiece      = fatPerPiece,
            PieceLabel       = pieceLabel,
            GramsPerPiece    = gramsPerPiece,
            IsActive         = true,
            CreatedAt        = DateTime.UtcNow
        };
    }

    public void Deactivate() => IsActive = false;
    public void Activate()   => IsActive = true;

    public void Update(
        string name,
        FoodMeasurementUnit unit,
        decimal? caloriesPer100g,
        decimal? proteinPer100g,
        decimal? carbsPer100g,
        decimal? fatPer100g,
        decimal? caloriesPerPiece,
        decimal? proteinPerPiece,
        decimal? carbsPerPiece,
        decimal? fatPerPiece,
        string? pieceLabel,
        decimal? gramsPerPiece)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Besin adı boş olamaz.");

        Name = name;
        Unit = unit;

        if (unit == FoodMeasurementUnit.Grams)
        {
            if (caloriesPer100g is null || proteinPer100g is null || carbsPer100g is null || fatPer100g is null)
                throw new ValidationException("Grams birimli besinde tüm 100g macro alanları zorunludur.");

            CaloriesPer100g = caloriesPer100g;
            ProteinPer100g  = proteinPer100g;
            CarbsPer100g    = carbsPer100g;
            FatPer100g      = fatPer100g;

            CaloriesPerPiece = null;
            ProteinPerPiece  = null;
            CarbsPerPiece    = null;
            FatPerPiece      = null;
            PieceLabel       = null;
            GramsPerPiece    = null;
        }
        else
        {
            if (caloriesPerPiece is null || proteinPerPiece is null || carbsPerPiece is null || fatPerPiece is null)
                throw new ValidationException("Piece birimli besinde tüm adet macro alanları zorunludur.");
            if (string.IsNullOrWhiteSpace(pieceLabel))
                throw new ValidationException("Piece birimli besinde 'pieceLabel' zorunludur.");

            CaloriesPerPiece = caloriesPerPiece;
            ProteinPerPiece  = proteinPerPiece;
            CarbsPerPiece    = carbsPerPiece;
            FatPerPiece      = fatPerPiece;
            PieceLabel       = pieceLabel;
            GramsPerPiece    = gramsPerPiece;

            CaloriesPer100g = null;
            ProteinPer100g  = null;
            CarbsPer100g    = null;
            FatPer100g      = null;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>EF HasData seeding — grams-based.</summary>
    public static Food SeedGrams(
        Guid id,
        string name,
        decimal caloriesPer100g,
        decimal proteinPer100g,
        decimal carbsPer100g,
        decimal fatPer100g)
    {
        return new Food
        {
            Id              = id,
            Name            = name,
            Unit            = FoodMeasurementUnit.Grams,
            CaloriesPer100g = caloriesPer100g,
            ProteinPer100g  = proteinPer100g,
            CarbsPer100g    = carbsPer100g,
            FatPer100g      = fatPer100g,
            IsActive        = true,
            CreatedAt       = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
    }

    /// <summary>EF HasData seeding — piece-based.</summary>
    public static Food SeedPiece(
        Guid id,
        string name,
        decimal caloriesPerPiece,
        decimal proteinPerPiece,
        decimal carbsPerPiece,
        decimal fatPerPiece,
        string pieceLabel,
        decimal? gramsPerPiece = null)
    {
        return new Food
        {
            Id               = id,
            Name             = name,
            Unit             = FoodMeasurementUnit.Piece,
            CaloriesPerPiece = caloriesPerPiece,
            ProteinPerPiece  = proteinPerPiece,
            CarbsPerPiece    = carbsPerPiece,
            FatPerPiece      = fatPerPiece,
            PieceLabel       = pieceLabel,
            GramsPerPiece    = gramsPerPiece,
            IsActive         = true,
            CreatedAt        = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
    }
}
