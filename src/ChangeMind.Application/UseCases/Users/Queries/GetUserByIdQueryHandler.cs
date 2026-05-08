namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Exceptions;
using Microsoft.Extensions.Options;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    ICacheService cache,
    IOptions<CacheOptions> cacheOptions) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var key = CacheKeys.User(request.UserId);

        var cached = await cache.GetAsync<UserDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await userRepository.GetUserByIdWithFitnessGoal(request.UserId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        await cache.SetAsync(key, dto, TimeSpan.FromSeconds(_cacheOptions.UserTtlSeconds), cancellationToken);

        return dto;
    }
}
