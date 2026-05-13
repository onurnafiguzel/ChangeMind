namespace ChangeMind.Application.UseCases.Foods.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record ListFoodsQuery : IRequest<List<FoodDto>>;
