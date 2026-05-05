namespace ChangeMind.Application.UseCases.FitnessGoals.Commands;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class DeleteFitnessGoalCommandHandler(
    IFitnessGoalRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteFitnessGoalCommand>
{
    public async Task Handle(DeleteFitnessGoalCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Fitness goal '{request.Id}' not found.");

        item.Deactivate();
        await repository.UpdateAsync(item);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
