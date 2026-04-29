namespace ChangeMind.UnitTests.Application;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.Payments.Queries;
using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Exceptions;
using FluentAssertions;
using Moq;

public class GetPaymentQueryHandlerTests
{
    private readonly Mock<IPaymentRepository> _paymentRepo = new();
    private readonly GetPaymentQueryHandler   _handler;

    public GetPaymentQueryHandlerTests()
    {
        _handler = new GetPaymentQueryHandler(_paymentRepo.Object);
    }

    [Fact]
    public async Task Handle_WithValidId_ShouldReturnPaymentDto()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var payment   = Payment.Create(Guid.NewGuid(), Guid.NewGuid(), 200m, "Test ödemesi");

        _paymentRepo.Setup(r => r.GetByIdAsync(paymentId)).ReturnsAsync(payment);

        // Act
        var result = await _handler.Handle(new GetPaymentQuery(paymentId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Amount.Should().Be(200m);
        result.Description.Should().Be("Test ödemesi");
        result.TransactionId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var missingId = Guid.NewGuid();
        _paymentRepo.Setup(r => r.GetByIdAsync(missingId)).ReturnsAsync((Payment?)null);

        // Act & Assert
        await _handler.Invoking(h => h.Handle(new GetPaymentQuery(missingId), CancellationToken.None))
                      .Should()
                      .ThrowAsync<NotFoundException>()
                      .WithMessage($"*{missingId}*");
    }
}
