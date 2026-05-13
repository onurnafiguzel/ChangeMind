namespace ChangeMind.Application.UseCases.NutritionPlans.Commands;

using MediatR;
using System.Text.Json;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class CreateNutritionPlanCommandHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IFoodRepository foodRepository,
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    IWaitingUserRepository waitingUserRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateNutritionPlanCommand, Guid>
{
    public async Task<Guid> Handle(CreateNutritionPlanCommand request, CancellationToken cancellationToken)
    {
        _ = await coachRepository.GetByIdAsync(request.CoachId)
            ?? throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        _ = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        // Resolve waiting record — user must be in waiting queue (payment completed)
        var waitingUser = await waitingUserRepository.GetByUserIdAsync(request.UserId)
            ?? throw new NotFoundException(
                $"Waiting record for user '{request.UserId}' not found. Payment may be incomplete.");

        // Duplicate guard: nutrition plan already assigned — do not allow another one
        if (waitingUser.HasNutritionPlan)
            throw new ConflictException(
                $"User '{request.UserId}' already has an active nutrition plan assigned.");

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

        var json = JsonSerializer.Serialize(request.Days);
        var plan = NutritionPlan.Create(
            coachId:       request.CoachId,
            userId:        request.UserId,
            title:         request.Title,
            description:   request.Description,
            dailyPlanJson: json);

        await nutritionPlanRepository.AddAsync(plan);

        // Mark waiting record: flips IsWaitingForAssignment=false only if training is also assigned
        waitingUser.MarkNutritionPlanAssigned();
        await waitingUserRepository.UpdateAsync(waitingUser);

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
