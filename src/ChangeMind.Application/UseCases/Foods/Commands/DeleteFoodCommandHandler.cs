namespace ChangeMind.Application.UseCases.Foods.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class DeleteFoodCommandHandler(
    IFoodRepository foodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteFoodCommand>
{
    public async Task Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var food = await foodRepository.GetByIdAsync(request.FoodId)
            ?? throw new NotFoundException($"Besin '{request.FoodId}' bulunamadı.");

        food.Deactivate();
        await foodRepository.UpdateAsync(food);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
