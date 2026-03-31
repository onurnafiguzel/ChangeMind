namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record CreateCoachCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    CoachSpecialization? Specialization = null) : IRequest<Guid>;
