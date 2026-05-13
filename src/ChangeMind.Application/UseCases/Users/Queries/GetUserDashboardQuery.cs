namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetUserDashboardQuery(Guid UserId) : IRequest<UserDashboardDto>;
