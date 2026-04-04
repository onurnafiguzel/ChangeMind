namespace ChangeMind.Application.UseCases.Auth.Commands;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class LoginCommandHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    IPasswordService passwordService,
    ITokenService tokenService,
    JwtSettings jwtSettings) : IRequestHandler<LoginCommand, AuthTokenResponse>
{
    public async Task<AuthTokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        var coach = user == null ? await coachRepository.GetByEmailAsync(request.Email) : null;

        if (user == null && coach == null)
            throw new UnauthorizedException("Invalid email or password.");

        var Id = user?.Id ?? coach!.Id;
        var email = user?.Email ?? coach!.Email;
        var role = user?.Role ?? coach!.Role;
        var passwordHash = user?.PasswordHash ?? coach!.PasswordHash;

        if (!passwordService.VerifyPassword(request.Password, passwordHash))
            throw new UnauthorizedException("Invalid email or password.");

        var (accessToken, refreshToken) = tokenService.GenerateTokens(Id, email, role);

        return new AuthTokenResponse
        {
            UserId = Id,
            Email = email,
            Role = role.ToString(),
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = jwtSettings.AccessTokenExpiryMinutes * 60
        };
    }
}
