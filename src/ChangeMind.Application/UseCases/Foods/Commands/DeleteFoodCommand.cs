namespace ChangeMind.Application.UseCases.Foods.Commands;

using MediatR;

public record DeleteFoodCommand(Guid FoodId) : IRequest;
