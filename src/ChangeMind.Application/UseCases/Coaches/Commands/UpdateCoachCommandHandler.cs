namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Repositories;

public class UpdateCoachCommandHandler(ICoachRepository coachRepository) : IRequestHandler<UpdateCoachCommand>
{
    public async Task Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId);
        if (coach == null)
            throw new KeyNotFoundException($"Coach with ID '{request.CoachId}' not found.");

        coach.Update(request.FirstName, request.LastName, request.Specialization);
        await coachRepository.UpdateAsync(coach);
    }
}
