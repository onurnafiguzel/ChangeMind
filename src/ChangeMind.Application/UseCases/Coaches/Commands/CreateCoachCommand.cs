namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record CreateCoachCommand(
    string Email,
    CoachSpecialization? Specialization = null) : IRequest<Guid>;
