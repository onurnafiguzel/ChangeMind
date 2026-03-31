namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new KeyNotFoundException($"User with ID '{request.UserId}' not found.");

        return MapToDto(user);
    }

    private static UserDto MapToDto(ChangeMind.Domain.Entities.User user)
    {
        return new UserDto
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
}
