namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetUserNutritionPlansQueryHandler(
    INutritionPlanRepository nutritionPlanRepository)
    : IRequestHandler<GetUserNutritionPlansQuery, List<NutritionPlanListItemDto>>
{
    public async Task<List<NutritionPlanListItemDto>> Handle(GetUserNutritionPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await nutritionPlanRepository.GetAllByUserIdAsync(request.UserId);
        return plans.Select(p => new NutritionPlanListItemDto
        {
            Id        = p.Id,
            Title     = p.Title,
            IsActive  = p.IsActive,
            CoachName = p.Coach == null ? string.Empty : $"{p.Coach.FirstName} {p.Coach.LastName}",
            CreatedAt = p.CreatedAt
        }).ToList();
    }
}
