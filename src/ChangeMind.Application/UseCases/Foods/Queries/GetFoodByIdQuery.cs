namespace ChangeMind.Application.UseCases.Foods.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetFoodByIdQuery(Guid FoodId) : IRequest<FoodDto>;
