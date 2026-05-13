namespace ChangeMind.Application.UseCases.Foods.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Exceptions;

public class UpdateFoodCommandHandler(
    IFoodRepository foodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateFoodCommand>
{
    public async Task Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var food = await foodRepository.GetByIdAsync(request.FoodId)
            ?? throw new NotFoundException($"Besin '{request.FoodId}' bulunamadı.");

        if (await foodRepository.ExistsByNameAsync(request.Name, excludingId: request.FoodId))
            throw new ConflictException($"'{request.Name}' isminde başka bir besin zaten var.");

        food.Update(
            name:             request.Name,
            unit:             request.Unit,
            caloriesPer100g:  request.CaloriesPer100g,
            proteinPer100g:   request.ProteinPer100g,
            carbsPer100g:     request.CarbsPer100g,
            fatPer100g:       request.FatPer100g,
            caloriesPerPiece: request.CaloriesPerPiece,
            proteinPerPiece:  request.ProteinPerPiece,
            carbsPerPiece:    request.CarbsPerPiece,
            fatPerPiece:      request.FatPerPiece,
            pieceLabel:       request.PieceLabel,
            gramsPerPiece:    request.GramsPerPiece);

        await foodRepository.UpdateAsync(food);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
