namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;

public record GetProfileCompletionStatusQuery(Guid UserId) : IRequest<bool>;
