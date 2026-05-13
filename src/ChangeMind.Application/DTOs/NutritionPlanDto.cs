namespace ChangeMind.Application.DTOs;

using ChangeMind.Domain.Enums;

public class MealItemDto
{
    public Guid FoodId { get; set; }
    public string FoodName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    /// <summary>"g" for grams-based foods or "adet" for piece-based foods.</summary>
    public string QuantityUnit { get; set; } = "g";
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }
}

public class MealDto
{
    public string Name { get; set; } = string.Empty;
    public List<MealItemDto> Items { get; set; } = new();
    public decimal TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
}

public class NutritionDayDto
{
    public NutritionDayType DayType { get; set; }
    public List<MealDto> Meals { get; set; } = new();
    public decimal TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
}

public class NutritionPlanDetailDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CoachId { get; set; }
    public string CoachName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int VersionNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<NutritionDayDto> Days { get; set; } = new();
}

public class NutritionPlanListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string CoachName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
