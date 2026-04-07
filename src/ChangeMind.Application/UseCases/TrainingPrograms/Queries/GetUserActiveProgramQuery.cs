namespace ChangeMind.Application.UseCases.TrainingPrograms.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetUserActiveProgramQuery : IRequest<ActiveProgramDetailDto?>
{
    public Guid UserId { get; set; }
}
