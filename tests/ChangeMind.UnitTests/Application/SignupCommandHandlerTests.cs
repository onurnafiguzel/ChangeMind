namespace ChangeMind.UnitTests.Application;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Application.UseCases.Auth.Commands;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using FluentAssertions;
using Moq;

public class SignupCommandHandlerTests
{
    private readonly Mock<IUserRepository>  _userRepo    = new();
    private readonly Mock<IPasswordService> _passwordSvc = new();
    private readonly Mock<ITokenService>    _tokenSvc    = new();
    private readonly Mock<IUnitOfWork>      _unitOfWork  = new();
    private readonly JwtSettings            _jwtSettings = new() { AccessTokenExpiryMinutes = 15 };
    private readonly SignupCommandHandler   _handler;

    public SignupCommandHandlerTests()
    {
        _handler = new SignupCommandHandler(
            _userRepo.Object,
            _passwordSvc.Object,
            _tokenSvc.Object,
            _unitOfWork.Object,
            _jwtSettings);
    }

    // Başarılı kayıt: geçerli email ve parola → token response döner.
    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnAuthTokenResponse()
    {
        // Arrange
        const string email       = "yeni@example.com";
        const string password    = "G3c3rli!Parola";
        const string hash        = "hashed";
        const string accessToken = "access.token";
        const string refreshToken = "refresh.token";

        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(password)).Returns(hash);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _tokenSvc
            .Setup(t => t.GenerateTokens(It.IsAny<Guid>(), email, It.IsAny<UserRole>()))
            .Returns((accessToken, refreshToken));

        var command = new SignupCommand(email, password);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<AuthTokenResponse>();
        result.Email.Should().Be(email);
        result.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(refreshToken);
        result.UserId.Should().NotBe(Guid.Empty);
        result.ExpiresIn.Should().Be(_jwtSettings.AccessTokenExpiryMinutes * 60);
    }

    // Role her zaman "User" olmalı.
    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnUserRole()
    {
        // Arrange
        const string email = "role@example.com";

        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hash");
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _tokenSvc
            .Setup(t => t.GenerateTokens(It.IsAny<Guid>(), email, UserRole.User))
            .Returns(("at", "rt"));

        var command = new SignupCommand(email, "parola");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Role.Should().Be(UserRole.User.ToString());
    }

    // Email zaten kayıtlıysa DuplicateEmailException fırlatılmalı.
    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        const string email = "mevcut@example.com";
        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(true);

        var command = new SignupCommand(email, "herhangi");

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should()
                      .ThrowAsync<DuplicateEmailException>()
                      .WithMessage($"*{email}*");
    }

    // Email zaten varsa veritabanına kayıt denenmemeli.
    [Fact]
    public async Task Handle_WithExistingEmail_ShouldNeverCallAddOrSave()
    {
        // Arrange
        _userRepo.Setup(r => r.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        var command = new SignupCommand("mevcut@example.com", "herhangi");

        // Act
        try { await _handler.Handle(command, CancellationToken.None); } catch { }

        // Assert
        _userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never());
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    // Parola hash'lenerek kaydedilmeli, düz metin gitmemeli.
    [Fact]
    public async Task Handle_ShouldHashPasswordBeforeSaving()
    {
        // Arrange
        const string email    = "hash@example.com";
        const string password = "D0gr3Parola!";
        const string hash     = "sha256_degeri";

        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(password)).Returns(hash);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _tokenSvc
            .Setup(t => t.GenerateTokens(It.IsAny<Guid>(), email, It.IsAny<UserRole>()))
            .Returns(("at", "rt"));

        var command = new SignupCommand(email, password);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepo.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.PasswordHash == hash &&
            u.Email == email
        )), Times.Once());
    }

    // Token servisi tam olarak bir kez, doğru parametrelerle çağrılmalı.
    [Fact]
    public async Task Handle_ShouldCallGenerateTokensOnce()
    {
        // Arrange
        const string email = "token@example.com";

        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hash");
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _tokenSvc
            .Setup(t => t.GenerateTokens(It.IsAny<Guid>(), email, UserRole.User))
            .Returns(("at", "rt"));

        var command = new SignupCommand(email, "parola");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _tokenSvc.Verify(t => t.GenerateTokens(It.IsAny<Guid>(), email, UserRole.User), Times.Once());
    }

    // Başarılı kayıt sonrası tam olarak bir kez SaveChangesAsync çağrılmalı.
    [Fact]
    public async Task Handle_ShouldCallSaveChangesOnce()
    {
        // Arrange
        _userRepo.Setup(r => r.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hash");
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _tokenSvc
            .Setup(t => t.GenerateTokens(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserRole>()))
            .Returns(("at", "rt"));

        var command = new SignupCommand("test@example.com", "parola");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
