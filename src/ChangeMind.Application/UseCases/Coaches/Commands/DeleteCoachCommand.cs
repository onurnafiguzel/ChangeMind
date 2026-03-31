namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;

public class DeleteCoachCommand : IRequest
{
    public Guid CoachId { get; set; }
}
