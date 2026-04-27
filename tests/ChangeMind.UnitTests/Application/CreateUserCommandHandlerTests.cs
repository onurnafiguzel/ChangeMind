namespace ChangeMind.UnitTests.Application;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Application.UseCases.Users.Commands;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;
using FluentAssertions;
using Moq;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository>  _userRepo     = new();
    private readonly Mock<IPasswordService> _passwordSvc  = new();
    private readonly Mock<IUnitOfWork>      _unitOfWork   = new();
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(
            _userRepo.Object,
            _passwordSvc.Object,
            _unitOfWork.Object);
    }

    // Başarılı kayıt: geçerli email ve parola → yeni kullanıcı ID'si döner.
    [Fact]
    public async Task Handle_WithValidEmailAndPassword_ShouldReturnNewUserId()
    {
        // Arrange
        const string email    = "yeni@example.com";
        const string password = "G3c3rli!Parola";
        const string hash     = "hashed_password";

        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(password)).Returns(hash);
        _userRepo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateUserCommand(email, password);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    // Aynı email zaten kayıtlıysa DuplicateEmailException fırlatılmalı.
    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        const string email = "mevcut@example.com";
        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(true);

        var command = new CreateUserCommand(email, "herhangi");

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

        var command = new CreateUserCommand("mevcut@example.com", "herhangi");

        // Act
        try { await _handler.Handle(command, CancellationToken.None); } catch { }

        // Assert
        _userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never());
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    // Parola servisinin döndürdüğü hash, AddAsync'e iletilen kullanıcıya atanmış olmalı.
    [Fact]
    public async Task Handle_ShouldHashPasswordBeforeSaving()
    {
        // Arrange
        const string email    = "hash@example.com";
        const string password = "G3c3rli!";
        const string hash     = "sha256_hash_degeri";

        _userRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(password)).Returns(hash);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateUserCommand(email, password);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert: AddAsync'e gönderilen User'ın PasswordHash = hash olmalı
        _userRepo.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.PasswordHash == hash &&
            u.Email == email
        )), Times.Once());
    }

    // Başarılı kayıt sonrası tam olarak bir kez SaveChangesAsync çağrılmalı.
    [Fact]
    public async Task Handle_ShouldCallSaveChangesOnce()
    {
        // Arrange
        _userRepo.Setup(r => r.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _passwordSvc.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hash");
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateUserCommand("test@example.com", "parola");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}
