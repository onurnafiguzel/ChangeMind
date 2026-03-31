namespace ChangeMind.Application.UseCases.Auth.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Entities;

public class SignupCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    ITokenService tokenService,
    JwtSettings jwtSettings) : IRequestHandler<SignupCommand, AuthTokenResponse>
{
    public async Task<AuthTokenResponse> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(request.Email))
            throw new InvalidOperationException($"A user with email '{request.Email}' already exists.");

        var passwordHash = passwordService.HashPassword(request.Password);
        var user = User.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName);

        await userRepository.AddAsync(user);

        var (accessToken, refreshToken) = tokenService.GenerateTokens(user.Id, user.Email, user.Role);

        return new AuthTokenResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.ToString(),
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = jwtSettings.AccessTokenExpiryMinutes * 60
        };
    }
}
