namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record UpdateNutritionPlanCommand(
    Guid PlanId,
    string Title,
    string? Description,
    Dictionary<NutritionDayType, List<MealInput>> Days) : IRequest;
