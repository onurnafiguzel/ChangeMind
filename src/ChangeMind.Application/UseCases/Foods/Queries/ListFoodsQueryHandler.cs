namespace ChangeMind.Application.UseCases.Foods.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class ListFoodsQueryHandler(IFoodRepository foodRepository)
    : IRequestHandler<ListFoodsQuery, List<FoodDto>>
{
    public async Task<List<FoodDto>> Handle(ListFoodsQuery request, CancellationToken cancellationToken)
    {
        var foods = await foodRepository.ListActiveAsync();
        return foods.Select(f => new FoodDto
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
        }).ToList();
    }
}
