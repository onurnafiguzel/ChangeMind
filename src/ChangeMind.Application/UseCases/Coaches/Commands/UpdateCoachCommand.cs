namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Domain.Enums;

public record UpdateCoachCommand(
    Guid CoachId,
    string FirstName,
    string LastName,
    CoachSpecialization? Specialization = null) : IRequest;
