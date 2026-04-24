namespace ChangeMind.UnitTests.Application;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Application.UseCases.Payments.Commands;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Domain.Exceptions;
using FluentAssertions;
using Moq;

// Moq — gerçek bir sınıfın yerine geçen sahte (mock) nesne oluşturur.
//
// Neden lazım?
// ProcessPaymentCommandHandler çalışmak için veritabanına ihtiyaç duyar.
// Unit test'te gerçek DB bağlantısı açmak istemiyoruz:
//   - Yavaş olur
//   - Test ortamında DB olmayabilir
//   - Testin başarısı DB durumuna bağımlı olur
//
// Moq ile IUserRepository, IPackageRepository gibi interface'lerin
// sahte versiyonlarını oluşturuyoruz. Gerçek DB yerine bu sahte nesneler kullanılıyor.
//
// Temel söz dizimi:
//   var mock = new Mock<IArayuz>();
//   mock.Setup(m => m.Metod(arg)).ReturnsAsync(deger);   ← bu çağrı şunu dönsün
//   mock.Verify(m => m.Metod(arg), Times.Once());         ← bu çağrı gerçekleşti mi?

public class ProcessPaymentCommandHandlerTests
{
    // Her testte yeniden kullanılacak mock'lar ve handler burada tanımlanır.
    // Bu sayede her test metodunda tekrar tekrar oluşturmaya gerek kalmaz.
    private readonly Mock<IUserRepository>        _userRepo        = new();
    private readonly Mock<IPackageRepository>     _packageRepo     = new();
    private readonly Mock<IPaymentRepository>     _paymentRepo     = new();
    private readonly Mock<IWaitingUserRepository> _waitingUserRepo = new();
    private readonly Mock<IUnitOfWork>            _unitOfWork      = new();
    private readonly ProcessPaymentCommandHandler _handler;

    public ProcessPaymentCommandHandlerTests()
    {
        // Handler'ı mock bağımlılıklarıyla oluştur.
        // Gerçek implementasyon yerine mock'lar inject ediliyor.
        _handler = new ProcessPaymentCommandHandler(
            _paymentRepo.Object,
            _userRepo.Object,
            _packageRepo.Object,
            _waitingUserRepo.Object,
            _unitOfWork.Object);
    }

    // -----------------------------------------------------------------------
    // Test 1 — Başarılı ödeme: Moq Setup + FluentAssertions
    //
    // Senaryo: Geçerli user ve package var, ödeme başarıyla tamamlanmalı.
    //
    // mock.Setup(...)       → bu metod çağrıldığında şunu dön
    // mock.Object          → mock'un gerçek nesne gibi kullanılabilen hali
    // -----------------------------------------------------------------------
    [Fact]
    public async Task Handle_WithValidUserAndPackage_ShouldReturnSuccess()
    {
        // Arrange
        var userId    = Guid.NewGuid();
        var packageId = Guid.NewGuid();

        var user    = User.Create("ali@example.com", "hash");
        var package = Package.Create("Premium", "Açıklama", price: 200m, durationDays: 30, PackageType.Premium);

        // Mock'lara talimat ver: "Bu argümanla çağrılırsa şunu dön"
        _userRepo.Setup(r => r.GetByIdAsync(userId))
                 .ReturnsAsync(user);

        _packageRepo.Setup(r => r.GetByIdAsync(packageId))
                    .ReturnsAsync(package);

        _waitingUserRepo.Setup(r => r.GetByUserIdAsync(userId))
                        .ReturnsAsync((WaitingUser?)null);  // waiting user kaydı yok

        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        var command = new ProcessPaymentCommand(userId, packageId, Amount: 200m);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert — FluentAssertions ile
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.PaymentId.Should().NotBe(Guid.Empty);
        result.Message.Should().Contain("Payment processed successfully");
    }

    // -----------------------------------------------------------------------
    // Test 2 — Kullanıcı bulunamadı: exception fırlatılmalı
    //
    // Senaryo: Geçersiz userId → handler NotFoundException fırlatmalı.
    //
    // mock.Setup(...).ReturnsAsync(null) → kayıt yok gibi davran
    // FluentAssertions: .ThrowAsync<T>() ile exception testi
    // -----------------------------------------------------------------------
    [Fact]
    public async Task Handle_WithInvalidUserId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidUserId = Guid.NewGuid();

        // Mock: bu user ID ile sorgu yapılırsa null dön (bulunamadı)
        _userRepo.Setup(r => r.GetByIdAsync(invalidUserId))
                 .ReturnsAsync((User?)null);

        var command = new ProcessPaymentCommand(invalidUserId, Guid.NewGuid(), Amount: 100m);

        // Act & Assert — exception fırlatıldığını doğrula
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should()
                      .ThrowAsync<NotFoundException>()
                      .WithMessage($"*{invalidUserId}*");  // mesaj userId içermeli
    }

    // -----------------------------------------------------------------------
    // Test 3 — Paket bulunamadı: exception fırlatılmalı
    //
    // Senaryo: User var ama Package yok → NotFoundException.
    //
    // mock.Verify(...) → bu metodun gerçekten çağrıldığını doğrula
    // Times.Never()    → hiç çağrılmadığını doğrula
    // -----------------------------------------------------------------------
    [Fact]
    public async Task Handle_WithInvalidPackageId_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId        = Guid.NewGuid();
        var invalidPkgId  = Guid.NewGuid();
        var user          = User.Create("test@example.com", "hash");

        _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        // Package bulunamadı
        _packageRepo.Setup(r => r.GetByIdAsync(invalidPkgId))
                    .ReturnsAsync((Package?)null);

        var command = new ProcessPaymentCommand(userId, invalidPkgId, Amount: 50m);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should()
                      .ThrowAsync<NotFoundException>();

        // Package bulunamadığı için SaveChanges hiç çağrılmamalı
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    // -----------------------------------------------------------------------
    // Test 4 — Amount 0 gelirse package fiyatı kullanılmalı
    //
    // Senaryo: request.Amount = 0 → payment.Amount = package.Price olmalı.
    //
    // mock.Verify(..., Times.Once()) → metodun tam bir kez çağrıldığını doğrula
    // -----------------------------------------------------------------------
    [Fact]
    public async Task Handle_WhenAmountIsZero_ShouldUsePackagePrice()
    {
        // Arrange
        var userId    = Guid.NewGuid();
        var packageId = Guid.NewGuid();
        var user      = User.Create("test@example.com", "hash");
        var package   = Package.Create("Starter", "Açıklama", price: 150m, durationDays: 15, PackageType.Basic);

        _userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _packageRepo.Setup(r => r.GetByIdAsync(packageId)).ReturnsAsync(package);
        _waitingUserRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync((WaitingUser?)null);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Amount = 0 gönderiyoruz
        var command = new ProcessPaymentCommand(userId, packageId, Amount: 0m);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();

        // AddAsync tam olarak bir kez çağrılmış olmalı
        _paymentRepo.Verify(r => r.AddAsync(It.Is<Payment>(p =>
            p.Amount == package.Price  // package.Price = 150m kullanıldı
        )), Times.Once());
    }
}
