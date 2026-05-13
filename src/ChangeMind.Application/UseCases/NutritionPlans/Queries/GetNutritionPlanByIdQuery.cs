namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetNutritionPlanByIdQuery(Guid PlanId) : IRequest<NutritionPlanDetailDto?>;
