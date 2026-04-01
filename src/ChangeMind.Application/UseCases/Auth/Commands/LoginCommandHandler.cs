namespace ChangeMind.Application.UseCases.Auth.Commands;

using MediatR;
using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    ITokenService tokenService,
    JwtSettings jwtSettings) : IRequestHandler<LoginCommand, AuthTokenResponse>
{
    public async Task<AuthTokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new InvalidOperationException("Invalid email or password.");

        if (!passwordService.VerifyPassword(request.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid email or password.");
        
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
