namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;

public class DeleteCoachCommandHandler(
    ICoachRepository coachRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCoachCommand>
{
    public async Task Handle(DeleteCoachCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId);
        if (coach == null)
            throw new KeyNotFoundException($"Coach with ID '{request.CoachId}' not found.");

        coach.Deactivate();
        await coachRepository.UpdateAsync(coach);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
