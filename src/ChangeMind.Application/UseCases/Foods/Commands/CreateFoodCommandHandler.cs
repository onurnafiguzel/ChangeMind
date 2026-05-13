namespace ChangeMind.Application.UseCases.Foods.Commands;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;

public class CreateFoodCommandHandler(
    IFoodRepository foodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateFoodCommand, Guid>
{
    public async Task<Guid> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        if (await foodRepository.ExistsByNameAsync(request.Name))
            throw new ConflictException($"'{request.Name}' isminde bir besin zaten var.");

        Food food = request.Unit == FoodMeasurementUnit.Grams
            ? Food.CreateGrams(
                name:             request.Name,
                caloriesPer100g:  request.CaloriesPer100g!.Value,
                proteinPer100g:   request.ProteinPer100g!.Value,
                carbsPer100g:     request.CarbsPer100g!.Value,
                fatPer100g:       request.FatPer100g!.Value)
            : Food.CreatePiece(
                name:             request.Name,
                caloriesPerPiece: request.CaloriesPerPiece!.Value,
                proteinPerPiece:  request.ProteinPerPiece!.Value,
                carbsPerPiece:    request.CarbsPerPiece!.Value,
                fatPerPiece:      request.FatPerPiece!.Value,
                pieceLabel:       request.PieceLabel!,
                gramsPerPiece:    request.GramsPerPiece);

        await foodRepository.AddAsync(food);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return food.Id;
    }
}
