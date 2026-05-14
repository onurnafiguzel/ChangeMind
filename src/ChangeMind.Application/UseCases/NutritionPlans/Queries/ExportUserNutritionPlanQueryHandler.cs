namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.UseCases.TrainingPrograms.Queries;

public class ExportUserNutritionPlanQueryHandler(IMediator mediator)
    : IRequestHandler<ExportUserNutritionPlanQuery, ProgramExportResult?>
{
    public async Task<ProgramExportResult?> Handle(ExportUserNutritionPlanQuery request, CancellationToken cancellationToken)
    {
        var plan = await mediator.Send(new GetUserActiveNutritionPlanQuery(request.UserId), cancellationToken);
        if (plan is null)
            return null;

        var bytes = NutritionPlanExcelBuilder.Build(plan);
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
