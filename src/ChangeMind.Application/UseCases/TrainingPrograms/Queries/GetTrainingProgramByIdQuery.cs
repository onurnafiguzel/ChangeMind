namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetTrainingProgramByIdQuery : IRequest<ActiveProgramDetailDto?>
{
    public Guid ProgramId { get; set; }
}
