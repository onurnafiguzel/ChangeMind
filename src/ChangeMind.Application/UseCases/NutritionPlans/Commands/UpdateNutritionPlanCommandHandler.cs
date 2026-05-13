namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class UpdateNutritionPlanCommandHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IFoodRepository foodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateNutritionPlanCommand>
{
    public async Task Handle(UpdateNutritionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await nutritionPlanRepository.GetByIdAsync(request.PlanId)
            ?? throw new NotFoundException($"Nutrition plan '{request.PlanId}' not found.");

        if (request.Days is null || request.Days.Count == 0)
            throw new ValidationException("En az bir gün (WorkoutDay veya OffDay) tanımlanmalıdır.");

        var allItems = request.Days
            .SelectMany(d => d.Value.SelectMany(m => m.Items))
            .ToList();

        var foods  = await foodRepository.ListActiveAsync();
        var lookup = foods.ToDictionary(f => f.Id);

        foreach (var item in allItems)
        {
            if (!lookup.TryGetValue(item.FoodId, out var food))
                throw new ValidationException($"Besin '{item.FoodId}' bulunamadı veya aktif değil.");

            ValidateUnitMatch(food, item);
        }

        plan.UpdateMeta(request.Title, request.Description);
        plan.UpdateDailyPlan(JsonSerializer.Serialize(request.Days));

        await nutritionPlanRepository.UpdateAsync(plan);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateUnitMatch(Food food, MealItemInput item)
    {
        if (food.Unit == FoodMeasurementUnit.Piece)
        {
            if (item.Pieces is null || item.Pieces <= 0m)
                throw new ValidationException(
                    $"'{food.Name}' yalnızca adet ile ölçülebilir. 'pieces' alanını doldurun.");
            if (item.Grams is not null && item.Grams > 0m)
                throw new ValidationException(
                    $"'{food.Name}' adet bazlıdır; 'grams' alanı kullanılamaz.");
        }
        else
        {
            if (item.Grams is null || item.Grams <= 0m)
                throw new ValidationException(
                    $"'{food.Name}' yalnızca gram ile ölçülebilir. 'grams' alanını doldurun.");
            if (item.Pieces is not null && item.Pieces > 0m)
                throw new ValidationException(
                    $"'{food.Name}' gram bazlıdır; 'pieces' alanı kullanılamaz.");
        }
    }
}
