namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class CreateSelfNutritionPlanCommandHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IFoodRepository foodRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSelfNutritionPlanCommand, Guid>
{
    public async Task<Guid> Handle(CreateSelfNutritionPlanCommand request, CancellationToken cancellationToken)
    {
        _ = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        if (request.Days is null || request.Days.Count == 0)
            throw new ValidationException("En az bir gün (WorkoutDay veya OffDay) tanımlanmalıdır.");

        var allItems = request.Days
            .SelectMany(d => d.Value.SelectMany(m => m.Items))
            .ToList();

        if (allItems.Count == 0)
            throw new ValidationException("Beslenme planı en az bir besin içermelidir.");

        var foods  = await foodRepository.ListActiveAsync();
        var lookup = foods.ToDictionary(f => f.Id);

        foreach (var item in allItems)
        {
            if (!lookup.TryGetValue(item.FoodId, out var food))
                throw new ValidationException($"Besin '{item.FoodId}' bulunamadı veya aktif değil.");

            ValidateUnitMatch(food, item);
        }

        // Single Self rule: deactivate existing Self plans for this user.
        var activePlans = await nutritionPlanRepository.GetActiveByUserIdAsync(request.UserId);
        foreach (var existing in activePlans.Where(p => p.CreatedByType == CreatedByType.Self))
        {
            existing.Deactivate();
            await nutritionPlanRepository.UpdateAsync(existing);
        }

        var json = JsonSerializer.Serialize(request.Days);
        var plan = NutritionPlan.CreateBySelf(
            userId:        request.UserId,
            title:         request.Title,
            description:   request.Description,
            dailyPlanJson: json);

        await nutritionPlanRepository.AddAsync(plan);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return plan.Id;
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
