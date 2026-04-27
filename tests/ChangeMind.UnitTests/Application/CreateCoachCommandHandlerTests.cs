namespace ChangeMind.UnitTests.Application;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Application.UseCases.Coaches.Commands;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using FluentAssertions;
using Moq;

public class CreateCoachCommandHandlerTests
{
    private readonly Mock<ICoachRepository> _coachRepo   = new();
    private readonly Mock<IUserRepository>  _userRepo    = new();
    private readonly Mock<IPasswordService> _passwordSvc = new();
    private readonly Mock<IUnitOfWork>      _unitOfWork  = new();
    private readonly CreateCoachCommandHandler _handler;

    public CreateCoachCommandHandlerTests()
    {
        _handler = new CreateCoachCommandHandler(
            _coachRepo.Object,
            _userRepo.Object,
            _passwordSvc.Object,
            _unitOfWork.Object);
    }

    private static User MakeUser(string email = "kullanici@example.com")
        => User.Create(email, "hash", "Ali", "Yılmaz");

    // Başarılı senaryo: user var, coach emaili kayıtlı değil → yeni coach ID'si döner.
    [Fact]
    public async Task Handle_WithValidUserAndUniqueEmail_ShouldReturnNewCoachId()
    {
        // Arrange
        const string email = "kullanici@example.com";
        var user = MakeUser(email);

        _coachRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _userRepo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateCoachCommand(email, CoachSpecialization.Strength);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    // Coach emaili zaten kayıtlıysa DuplicateEmailException fırlatılmalı.
    [Fact]
    public async Task Handle_WithExistingCoachEmail_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        const string email = "mevcut_koc@example.com";
        _coachRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(true);

        var command = new CreateCoachCommand(email);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should()
                      .ThrowAsync<DuplicateEmailException>()
                      .WithMessage($"*{email}*");
    }

    // İlgili user bulunamazsa NotFoundException fırlatılmalı.
    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        const string email = "yok@example.com";
        _coachRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _userRepo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((User?)null);

        var command = new CreateCoachCommand(email);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should()
                      .ThrowAsync<NotFoundException>();
    }

    // Başarılı coach oluşturmada mevcut user Deactivate edilmeli.
    [Fact]
    public async Task Handle_ShouldDeactivateUserWhenCoachCreated()
    {
        // Arrange
        const string email = "donusecek@example.com";
        var user = MakeUser(email);

        _coachRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _userRepo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateCoachCommand(email, CoachSpecialization.Cardio);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert: user artık pasif olmalı
        user.IsActive.Should().BeFalse();
        _userRepo.Verify(r => r.UpdateAsync(user), Times.Once());
    }

    // Coach oluşturulurken Specialization null geçilebilmeli.
    [Fact]
    public async Task Handle_WithNullSpecialization_ShouldSucceed()
    {
        // Arrange
        const string email = "uzman_yok@example.com";
        var user = MakeUser(email);

        _coachRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _userRepo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateCoachCommand(email, Specialization: null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        _coachRepo.Verify(r => r.AddAsync(It.Is<Coach>(c =>
            c.Specialization == null &&
            c.Email == email
        )), Times.Once());
    }

    // Hata senaryolarında SaveChangesAsync çağrılmamalı.
    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldNeverSave()
    {
        // Arrange
        const string email = "hata@example.com";
        _coachRepo.Setup(r => r.ExistsAsync(email)).ReturnsAsync(false);
        _userRepo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((User?)null);

        var command = new CreateCoachCommand(email);

        // Act
        try { await _handler.Handle(command, CancellationToken.None); } catch { }

        // Assert
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }
}
