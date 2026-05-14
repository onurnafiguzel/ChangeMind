namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;

public record ProgramExportResult(string FileName, byte[] Content);

public record ExportTrainingProgramQuery(Guid ProgramId) : IRequest<ProgramExportResult?>;
