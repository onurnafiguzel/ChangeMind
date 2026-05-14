namespace ChangeMind.Application.UseCases.NutritionPlans.Queries;

using MediatR;
using ChangeMind.Application.UseCases.TrainingPrograms.Queries;

public record ExportUserNutritionPlanQuery(Guid UserId) : IRequest<ProgramExportResult?>;
