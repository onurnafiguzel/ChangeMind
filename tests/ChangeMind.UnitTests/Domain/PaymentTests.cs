namespace ChangeMind.UnitTests.Domain;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using FluentAssertions;

// FluentAssertions — Assert.Equal yerine daha okunabilir bir söz dizimi sağlar.
// "Should" ile başlayan metodlar cümle gibi okunur:
//   Assert.Equal(x, y)          →  x.Should().Be(y)
//   Assert.NotNull(x)           →  x.Should().NotBeNull()
//   Assert.True(x > 0)          →  x.Should().BePositive()

public class PaymentTests
{
    // -----------------------------------------------------------------------
    // Test 1 — FluentAssertions ile temel doğrulama
    //
    // Payment.Create çağrıldığında alanlar doğru set edilmeli.
    // Assert.Equal yerine .Should().Be() kullanıyoruz — daha açık hata mesajı verir.
    // -----------------------------------------------------------------------
    [Fact]
    public void Create_ShouldSetFieldsCorrectly()
    {
        // Arrange
        var userId    = Guid.NewGuid();
        var packageId = Guid.NewGuid();
        var amount    = 250m;

        // Act
        var payment = Payment.Create(userId, packageId, amount, description: "Test ödemesi");

        // Assert — FluentAssertions
        payment.UserId.Should().Be(userId);
        payment.PackageId.Should().Be(packageId);
        payment.Amount.Should().Be(amount);
        payment.Status.Should().Be(PaymentStatus.Pending);   // başlangıçta Pending olmalı
        payment.Id.Should().NotBe(Guid.Empty);               // Id üretilmeli
        payment.CompletedAt.Should().BeNull();                // henüz tamamlanmadı
    }

    // -----------------------------------------------------------------------
    // Test 2 — Durum geçiş testi: MarkAsCompleted sonrası ne olmalı?
    //
    // FluentAssertions'ın string kontrolü: .Be(), .StartWith(), .Contain()
    // -----------------------------------------------------------------------
    [Fact]
    public void MarkAsCompleted_ShouldChangeStatusAndSetTransactionId()
    {
        // Arrange
        var payment       = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 100m);
        var transactionId = "TXN-12345";

        // Act
        payment.MarkAsCompleted(transactionId, 30);

        // Assert
        payment.Status.Should().Be(PaymentStatus.Completed);
        payment.TransactionId.Should().Be(transactionId);
        payment.CompletedAt.Should().NotBeNull();
        payment.CompletedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
    }

    // -----------------------------------------------------------------------
    // Test 3 — İdempotency key doğru formatta kaydediliyor mu?
    //
    // Guid → string("D") dönüşümünü test ediyoruz.
    // .Should().MatchRegex() ile format doğrulaması.
    // -----------------------------------------------------------------------
    [Fact]
    public void Create_WithIdempotencyKey_ShouldStoreAsFormattedString()
    {
        // Arrange
        var idempotencyKey = Guid.NewGuid();

        // Act
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 50m, idempotencyKey: idempotencyKey);

        // Assert — "D" format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        payment.IdempotencyKey.Should().NotBeNullOrEmpty();
        payment.IdempotencyKey.Should().Be(idempotencyKey.ToString("D"));
        payment.IdempotencyKey.Should().MatchRegex(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$");
    }

    // -----------------------------------------------------------------------
    // Test 4 — [Theory] ile birden fazla durum geçişi
    //
    // MarkAsFailed ve MarkAsRefunded farklı Status değerleri üretmeli.
    // Enum değerini parametre olarak geçmek için object[] kullanıyoruz.
    // -----------------------------------------------------------------------
    [Fact]
    public void MarkAsFailed_FromPending_ShouldSetFailedStatus()
    {
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 75m);
        payment.MarkAsFailed();
        payment.Status.Should().Be(PaymentStatus.Failed);
    }

    [Fact]
    public void MarkAsRefunded_FromCompleted_ShouldSetRefundedStatus()
    {
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 75m);
        payment.MarkAsCompleted("TXN-REF", 30);
        payment.MarkAsRefunded();
        payment.Status.Should().Be(PaymentStatus.Refunded);
    }

    [Fact]
    public void MarkAsRefunded_FromPending_ShouldThrow()
    {
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 75m);
        var act = () => payment.MarkAsRefunded();
        act.Should().Throw<ChangeMind.Domain.Exceptions.InvalidStateTransitionException>();
    }

    [Fact]
    public void MarkAsCompleted_WhenAlreadyCompleted_ShouldThrow()
    {
        var payment = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 100m);
        payment.MarkAsCompleted("TXN-1", 30);
        var act = () => payment.MarkAsCompleted("TXN-2", 30);
        act.Should().Throw<ChangeMind.Domain.Exceptions.InvalidStateTransitionException>();
    }
}
