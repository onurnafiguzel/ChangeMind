namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class FoodDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public FoodMeasurementUnit Unit { get; set; }

    // Per 100g (Unit=Grams)
    public decimal? CaloriesPer100g { get; set; }
    public decimal? ProteinPer100g { get; set; }
    public decimal? CarbsPer100g { get; set; }
    public decimal? FatPer100g { get; set; }

    // Per piece (Unit=Piece)
    public decimal? CaloriesPerPiece { get; set; }
    public decimal? ProteinPerPiece { get; set; }
    public decimal? CarbsPerPiece { get; set; }
    public decimal? FatPerPiece { get; set; }

    public string? PieceLabel { get; set; }
    public decimal? GramsPerPiece { get; set; }

    public bool IsActive { get; set; }
}
