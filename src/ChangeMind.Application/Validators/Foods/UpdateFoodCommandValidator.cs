namespace ChangeMind.Application.Validators.Foods;

using ChangeMind.Application.UseCases.Foods.Commands;
using ChangeMind.Domain.Enums;
using FluentValidation;

public sealed class UpdateFoodCommandValidator : AbstractValidator<UpdateFoodCommand>
{
    public UpdateFoodCommandValidator()
    {
        RuleFor(x => x.FoodId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Unit).IsInEnum();

        When(x => x.Unit == FoodMeasurementUnit.Grams, () =>
        {
            RuleFor(x => x.CaloriesPer100g).NotNull().GreaterThanOrEqualTo(0m);
            RuleFor(x => x.ProteinPer100g).NotNull().GreaterThanOrEqualTo(0m);
            RuleFor(x => x.CarbsPer100g).NotNull().GreaterThanOrEqualTo(0m);
            RuleFor(x => x.FatPer100g).NotNull().GreaterThanOrEqualTo(0m);

            RuleFor(x => x.CaloriesPerPiece).Null();
            RuleFor(x => x.ProteinPerPiece).Null();
            RuleFor(x => x.CarbsPerPiece).Null();
            RuleFor(x => x.FatPerPiece).Null();
            RuleFor(x => x.PieceLabel).Null();
            RuleFor(x => x.GramsPerPiece).Null();
        });

        When(x => x.Unit == FoodMeasurementUnit.Piece, () =>
        {
            RuleFor(x => x.CaloriesPerPiece).NotNull().GreaterThanOrEqualTo(0m);
            RuleFor(x => x.ProteinPerPiece).NotNull().GreaterThanOrEqualTo(0m);
            RuleFor(x => x.CarbsPerPiece).NotNull().GreaterThanOrEqualTo(0m);
            RuleFor(x => x.FatPerPiece).NotNull().GreaterThanOrEqualTo(0m);
            RuleFor(x => x.PieceLabel).NotEmpty().MaximumLength(100);
            RuleFor(x => x.GramsPerPiece).GreaterThan(0m).When(x => x.GramsPerPiece.HasValue);

            RuleFor(x => x.CaloriesPer100g).Null();
            RuleFor(x => x.ProteinPer100g).Null();
            RuleFor(x => x.CarbsPer100g).Null();
            RuleFor(x => x.FatPer100g).Null();
        });
    }
}
