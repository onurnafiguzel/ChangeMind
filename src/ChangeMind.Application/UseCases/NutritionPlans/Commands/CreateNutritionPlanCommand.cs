namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record CreateNutritionPlanCommand(
    Guid CoachId,
    Guid UserId,
    string Title,
    string? Description,
    Dictionary<NutritionDayType, List<MealInput>> Days) : IRequest<Guid>;

public record MealInput(string Name, List<MealItemInput> Items);

public record MealItemInput(Guid FoodId, decimal? Grams = null, decimal? Pieces = null);
