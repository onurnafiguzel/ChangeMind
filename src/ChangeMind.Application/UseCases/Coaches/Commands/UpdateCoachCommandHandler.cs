namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class UpdateCoachCommandHandler(
    ICoachRepository coachRepository,
    IUnitOfWork unitOfWork,
    ICacheService cache) : IRequestHandler<UpdateCoachCommand>
{
    public async Task Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByIdAsync(request.CoachId)
            ?? throw new NotFoundException($"Coach with ID '{request.CoachId}' not found.");

        coach.Update(request.FirstName, request.LastName, request.Specialization);
        await coachRepository.UpdateAsync(coach);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.RemoveAsync(CacheKeys.Coach(request.CoachId), cancellationToken);
    }
}
