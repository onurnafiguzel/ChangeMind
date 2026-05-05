namespace ChangeMind.Application.UseCases.WaitingUsers.Queries;

using ChangeMind.Application.DTOs;
using MediatR;

public record GetWaitingUserStatusQuery(Guid UserId) : IRequest<WaitingUserStatusDto>;
