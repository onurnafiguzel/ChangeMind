namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using System.Text.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.UseCases.NutritionPlans.Commands;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;

internal static class NutritionPlanMapper
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public static NutritionPlanDetailDto ToDetailDto(NutritionPlan plan, IReadOnlyList<Food> foods)
    {
        var foodLookup = foods.ToDictionary(f => f.Id);

        var dto = new NutritionPlanDetailDto
        {
            Id            = plan.Id,
            UserId        = plan.UserId,
            CoachId       = plan.CoachId,
            CoachName     = plan.Coach == null ? null : $"{plan.Coach.FirstName} {plan.Coach.LastName}",
            CreatedByType = plan.CreatedByType,
            Title         = plan.Title,
            Description   = plan.Description,
            IsActive      = plan.IsActive,
            VersionNumber = plan.VersionNumber,
            CreatedAt     = plan.CreatedAt,
            UpdatedAt     = plan.UpdatedAt,
            Days          = new List<NutritionDayDto>()
        };

        Dictionary<NutritionDayType, List<MealInput>>? days = null;
        try
        {
            days = JsonSerializer.Deserialize<Dictionary<NutritionDayType, List<MealInput>>>(plan.DailyPlanJson, JsonOpts);
        }
        catch
        {
            // ignore — return empty days
        }

        if (days == null) return dto;

        foreach (var (dayType, meals) in days)
        {
            var dayDto = new NutritionDayDto { DayType = dayType, Meals = new List<MealDto>() };

            foreach (var meal in meals)
            {
                var mealDto = new MealDto { Name = meal.Name, Items = new List<MealItemDto>() };
                foreach (var item in meal.Items)
                {
                    if (!foodLookup.TryGetValue(item.FoodId, out var food)) continue;

                    var entry = ComputeMacros(food, item);
                    mealDto.Items.Add(entry);
                    mealDto.TotalCalories += entry.Calories;
                    mealDto.TotalProtein  += entry.Protein;
                    mealDto.TotalCarbs    += entry.Carbs;
                    mealDto.TotalFat      += entry.Fat;
                }
                dayDto.Meals.Add(mealDto);
                dayDto.TotalCalories += mealDto.TotalCalories;
                dayDto.TotalProtein  += mealDto.TotalProtein;
                dayDto.TotalCarbs    += mealDto.TotalCarbs;
                dayDto.TotalFat      += mealDto.TotalFat;
            }

            dto.Days.Add(dayDto);
        }

        return dto;
    }

    private static MealItemDto ComputeMacros(Food food, MealItemInput item)
    {
        if (food.Unit == FoodMeasurementUnit.Piece)
        {
            var pieces = item.Pieces ?? 0m;
            return new MealItemDto
            {
                FoodId       = food.Id,
                FoodName     = food.Name,
                Quantity     = pieces,
                QuantityUnit = "adet",
                Calories     = Math.Round((food.CaloriesPerPiece ?? 0m) * pieces, 2),
                Protein      = Math.Round((food.ProteinPerPiece  ?? 0m) * pieces, 2),
                Carbs        = Math.Round((food.CarbsPerPiece    ?? 0m) * pieces, 2),
                Fat          = Math.Round((food.FatPerPiece      ?? 0m) * pieces, 2)
            };
        }

        var grams  = item.Grams ?? 0m;
        var factor = grams / 100m;
        return new MealItemDto
        {
            FoodId       = food.Id,
            FoodName     = food.Name,
            Quantity     = grams,
            QuantityUnit = "g",
            Calories     = Math.Round((food.CaloriesPer100g ?? 0m) * factor, 2),
            Protein      = Math.Round((food.ProteinPer100g  ?? 0m) * factor, 2),
            Carbs        = Math.Round((food.CarbsPer100g    ?? 0m) * factor, 2),
            Fat          = Math.Round((food.FatPer100g      ?? 0m) * factor, 2)
        };
    }
}
