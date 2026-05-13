namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;

public record ActivateNutritionPlanCommand(Guid PlanId) : IRequest;
public record DeactivateNutritionPlanCommand(Guid PlanId) : IRequest;
