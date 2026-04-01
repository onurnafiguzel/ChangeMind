namespace ChangeMind.Application.UseCases.Coaches.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Exceptions;

public class LoginCoachCommandHandler(
    ICoachRepository coachRepository,
    IPasswordService passwordService,
    ITokenService tokenService,
    JwtSettings jwtSettings) : IRequestHandler<LoginCoachCommand, AuthTokenResponse>
{
    public async Task<AuthTokenResponse> Handle(LoginCoachCommand request, CancellationToken cancellationToken)
    {
        var coach = await coachRepository.GetByEmailAsync(request.Email);
        if (coach == null)
            throw new UnauthorizedException("Invalid email or password.");

        if (!passwordService.VerifyPassword(request.Password, coach.PasswordHash))
            throw new UnauthorizedException("Invalid email or password.");

        var (accessToken, refreshToken) = tokenService.GenerateTokens(coach.Id, coach.Email, coach.Role);

        return new AuthTokenResponse
        {
            UserId = coach.Id,
            Email = coach.Email,
            Role = coach.Role.ToString(),
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = jwtSettings.AccessTokenExpiryMinutes * 60
        };
    }
}
