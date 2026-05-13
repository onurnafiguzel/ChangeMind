namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetNutritionPlanByIdQueryHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IFoodRepository foodRepository)
    : IRequestHandler<GetNutritionPlanByIdQuery, NutritionPlanDetailDto?>
{
    public async Task<NutritionPlanDetailDto?> Handle(GetNutritionPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await nutritionPlanRepository.GetByIdAsync(request.PlanId);
        if (plan == null) return null;

        var foods = await foodRepository.ListActiveAsync();
        return NutritionPlanMapper.ToDetailDto(plan, foods);
    }
}
