namespace ChangeMind.Application.Mappers;

using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Entities;

public static class CoachMapper
{
    public static CoachDto ToDto(Coach coach) => new()
    {
        Id = coach.Id,
        Email = coach.Email,
        FirstName = coach.FirstName,
        LastName = coach.LastName,
        Specialization = coach.Specialization?.ToString(),
        IsActive = coach.IsActive,
        CreatedAt = coach.CreatedAt,
        UpdatedAt = coach.UpdatedAt
    };
}
