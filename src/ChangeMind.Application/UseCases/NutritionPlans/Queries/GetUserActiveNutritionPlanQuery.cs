namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetUserActiveNutritionPlanQuery(Guid UserId) : IRequest<NutritionPlanDetailDto?>;
