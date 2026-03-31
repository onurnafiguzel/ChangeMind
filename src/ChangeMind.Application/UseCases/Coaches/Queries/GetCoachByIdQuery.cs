namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetCoachByIdQuery : IRequest<CoachDto>
{
    public Guid CoachId { get; set; }
}
