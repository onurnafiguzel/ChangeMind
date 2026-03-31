namespace ChangeMind.Application.UseCases.Users.Queries;

using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    public async Task<PagedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // Validate pagination
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1) request.PageSize = 10;

        var query = userRepository.GetAll(request.IsActiveOnly);

        // Count total at database level
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination and projection at database level
        var userDtos = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(user => new UserDto
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
            })
            .ToListAsync(cancellationToken);

        // Calculate total pages
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new PagedResult<UserDto>
        {
            Data = userDtos,
            Total = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
}
