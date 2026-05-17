namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.TrainingPrograms.Queries;

public class ExportNutritionPlanQueryHandler(
    INutritionPlanRepository nutritionPlanRepository,
    IFoodRepository foodRepository)
    : IRequestHandler<ExportNutritionPlanQuery, ProgramExportResult?>
{
    public async Task<ProgramExportResult?> Handle(ExportNutritionPlanQuery request, CancellationToken cancellationToken)
    {
        var plan = await nutritionPlanRepository.GetByIdAsync(request.PlanId);
        if (plan is null) return null;

        var foods = await foodRepository.ListActiveAsync();
        var dto = NutritionPlanMapper.ToDetailDto(plan, foods);

        var bytes = NutritionPlanExcelBuilder.Build(dto);
        var safeTitle = SanitizeFileName(string.IsNullOrWhiteSpace(plan.Title) ? "NutritionPlan" : plan.Title);
        var fileName = $"{safeTitle}_{DateTime.UtcNow:yyyyMMdd}.xlsx";

        return new ProgramExportResult(fileName, bytes);
    }

    private static string SanitizeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        return cleaned.Replace(' ', '_');
    }
}
