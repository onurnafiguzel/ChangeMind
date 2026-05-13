namespace ChangeMind.Application.UseCases.Foods.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;

public class GetFoodByIdQueryHandler(IFoodRepository foodRepository)
    : IRequestHandler<GetFoodByIdQuery, FoodDto>
{
    public async Task<FoodDto> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
    {
        var f = await foodRepository.GetByIdAsync(request.FoodId)
            ?? throw new NotFoundException($"Besin '{request.FoodId}' bulunamadı.");

        return new FoodDto
        {
            Id               = f.Id,
            Name             = f.Name,
            Unit             = f.Unit,
            CaloriesPer100g  = f.CaloriesPer100g,
            ProteinPer100g   = f.ProteinPer100g,
            CarbsPer100g     = f.CarbsPer100g,
            FatPer100g       = f.FatPer100g,
            CaloriesPerPiece = f.CaloriesPerPiece,
            ProteinPerPiece  = f.ProteinPerPiece,
            CarbsPerPiece    = f.CarbsPerPiece,
            FatPerPiece      = f.FatPerPiece,
            PieceLabel       = f.PieceLabel,
            GramsPerPiece    = f.GramsPerPiece,
            IsActive         = f.IsActive
        };
    }
}
