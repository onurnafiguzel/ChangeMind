namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class UpdateCoachCommandHandler(
    ICoachRepository coachRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCoachCommand>
{
    public async Task Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId);
        if (coach == null)
            throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        coach.Update(request.FirstName, request.LastName, request.Specialization);
        await coachRepository.UpdateAsync(coach);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
