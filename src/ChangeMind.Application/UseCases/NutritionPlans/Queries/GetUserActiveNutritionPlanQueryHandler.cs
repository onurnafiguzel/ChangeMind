namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Enums;

public class GetUserActiveNutritionPlanQueryHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IFoodRepository foodRepository)
    : IRequestHandler<GetUserActiveNutritionPlanQuery, NutritionPlanDetailDto?>
{
    public async Task<NutritionPlanDetailDto?> Handle(GetUserActiveNutritionPlanQuery request, CancellationToken cancellationToken)
    {
        var plans = await nutritionPlanRepository.GetActiveByUserIdAsync(request.UserId);
        if (plans.Count == 0) return null;

        // Coach-assigned plan takes precedence over Self-created.
        var selected =
            plans.FirstOrDefault(p => p.CreatedByType == CreatedByType.Coach)
            ?? plans.FirstOrDefault(p => p.CreatedByType == CreatedByType.Self);

        if (selected is null) return null;

        var foods = await foodRepository.ListActiveAsync();
        return NutritionPlanMapper.ToDetailDto(selected, foods);
    }
}
