namespace ChangeMind.Application.UseCases.Foods.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record UpdateFoodCommand(
    Guid FoodId,
    string Name,
    FoodMeasurementUnit Unit,
    decimal? CaloriesPer100g  = null,
    decimal? ProteinPer100g   = null,
    decimal? CarbsPer100g     = null,
    decimal? FatPer100g       = null,
    decimal? CaloriesPerPiece = null,
    decimal? ProteinPerPiece  = null,
    decimal? CarbsPerPiece    = null,
    decimal? FatPerPiece      = null,
    string?  PieceLabel       = null,
    decimal? GramsPerPiece    = null) : IRequest;
