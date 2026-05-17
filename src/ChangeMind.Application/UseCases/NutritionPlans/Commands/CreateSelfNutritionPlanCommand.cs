namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record CreateSelfNutritionPlanCommand(
    Guid UserId,
    string Title,
    string? Description,
    Dictionary<NutritionDayType, List<MealInput>> Days) : IRequest<Guid>;
