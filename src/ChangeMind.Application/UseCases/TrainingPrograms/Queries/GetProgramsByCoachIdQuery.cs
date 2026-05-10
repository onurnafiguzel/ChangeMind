namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetProgramsByCoachIdQuery : IRequest<List<CoachProgramListItemDto>>
{
    public Guid CoachId { get; set; }
}
