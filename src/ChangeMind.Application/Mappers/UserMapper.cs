namespace ChangeMind.Application.Mappers;

using ChangeMind.Application.DTOs;
using ChangeMind.Domain.Entities;

public static class UserMapper
{
    public static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Age = user.Age,
        Height = user.Height,
        Weight = user.Weight,
        Gender = user.Gender,
        FitnessGoal = user.FitnessGoal,
        FitnessLevel = user.FitnessLevel,
        CreatedAt = user.CreatedAt,
    };
}
