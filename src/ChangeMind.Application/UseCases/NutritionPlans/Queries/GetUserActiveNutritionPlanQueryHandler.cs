namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetUserActiveNutritionPlanQueryHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IFoodRepository foodRepository)
    : IRequestHandler<GetUserActiveNutritionPlanQuery, NutritionPlanDetailDto?>
{
    public async Task<NutritionPlanDetailDto?> Handle(GetUserActiveNutritionPlanQuery request, CancellationToken cancellationToken)
    {
        var plan = await nutritionPlanRepository.GetLatestActiveByUserIdAsync(request.UserId);
        if (plan == null) return null;

        var foods = await foodRepository.ListActiveAsync();
        return NutritionPlanMapper.ToDetailDto(plan, foods);
    }
}
