namespace ChangeMind.Application.UseCases.FitnessGoals.Commands;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class UpdateFitnessGoalCommandHandler(
    IFitnessGoalRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateFitnessGoalCommand>
{
    public async Task Handle(UpdateFitnessGoalCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Fitness goal '{request.Id}' not found.");

        if (!string.Equals(item.Name, request.Name, StringComparison.OrdinalIgnoreCase)
            && await repository.ExistsAsync(request.Name))
            throw new ConflictException($"A fitness goal named '{request.Name}' already exists.");

        item.Update(request.Name, request.Description);
        await repository.UpdateAsync(item);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
