namespace ChangeMind.Application.UseCases.FitnessGoals.Commands;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class CreateFitnessGoalCommandHandler(
    IFitnessGoalRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateFitnessGoalCommand, Guid>
{
    public async Task<Guid> Handle(CreateFitnessGoalCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsAsync(request.Name))
            throw new ConflictException($"A fitness goal named '{request.Name}' already exists.");

        var item = FitnessGoalItem.Create(request.Name, request.Description);
        await repository.AddAsync(item);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
