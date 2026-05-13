namespace ChangeMind.Application.UseCases.Coaches.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public record GetCoachDashboardQuery(Guid CoachId) : IRequest<CoachDashboardDto>;
