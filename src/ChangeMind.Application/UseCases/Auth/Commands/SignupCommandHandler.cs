namespace ChangeMind.Application.UseCases.Auth.Commands;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;
using MediatR;

public class SignupCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    JwtSettings jwtSettings) : IRequestHandler<SignupCommand, AuthTokenResponse>
{
    public async Task<AuthTokenResponse> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(request.Email))
            throw new DuplicateEmailException(request.Email);

        var passwordHash = passwordService.HashPassword(request.Password);
        var user = User.Create(email: request.Email, passwordHash: passwordHash);

        await userRepository.AddAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
