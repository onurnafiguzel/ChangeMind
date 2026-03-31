namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Repositories;

public class DeleteCoachCommandHandler(ICoachRepository coachRepository) : IRequestHandler<DeleteCoachCommand>
{
    public async Task Handle(DeleteCoachCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId);
        if (coach == null)
            throw new KeyNotFoundException($"Coach with ID '{request.CoachId}' not found.");

        coach.Deactivate();
        await coachRepository.UpdateAsync(coach);
    }
}
