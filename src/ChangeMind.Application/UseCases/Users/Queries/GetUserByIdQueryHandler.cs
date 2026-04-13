namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await userRepository.GetById(request.UserId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
                Height = u.Height,
                Weight = u.Weight,
                Gender = u.Gender,
                FitnessGoal = u.FitnessGoal,
                FitnessLevel = u.FitnessLevel,
                CreatedAt = u.CreatedAt,
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        return dto;
    }
}
