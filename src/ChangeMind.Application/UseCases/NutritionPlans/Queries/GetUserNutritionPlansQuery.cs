namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetUserNutritionPlansQuery(Guid UserId) : IRequest<List<NutritionPlanListItemDto>>;
