namespace ChangeMind.UnitTests.Application;

using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.Users.Queries;
using ChangeMind.Domain.Entities;
using ChangeMind.UnitTests.Helpers;
using FluentAssertions;
using Moq;

public class GetUsersQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly GetUsersQueryHandler  _handler;

    public GetUsersQueryHandlerTests()
    {
        _handler = new GetUsersQueryHandler(_userRepo.Object);
    }

    private static List<User> MakeUsers(int count)
        => Enumerable.Range(1, count)
            .Select(i => User.Create($"user{i}@example.com", "hash", $"Ad{i}", $"Soyad{i}"))
            .ToList();

    // Aktif kullanıcılar varken sayfalı sonuç döner.
    [Fact]
    public async Task Handle_WithExistingUsers_ShouldReturnPagedResult()
    {
        // Arrange
        var users = MakeUsers(5);
        _userRepo.Setup(r => r.GetAll(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(users));

        var query = new GetUsersQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(5);
        result.Total.Should().Be(5);
        result.Page.Should().Be(1);
    }

    // Sayfalama doğru çalışmalı: 3 kayıttan Page=2, PageSize=2 → 1 kayıt döner.
    [Fact]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var users = MakeUsers(3);
        _userRepo.Setup(r => r.GetAll(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(users));

        var query = new GetUsersQuery { Page = 2, PageSize = 2 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Total.Should().Be(3);
        result.TotalPages.Should().Be(2);
    }

    // Kayıt yokken boş liste ve sıfır total döner.
    [Fact]
    public async Task Handle_WithNoUsers_ShouldReturnEmptyResult()
    {
        // Arrange
        _userRepo.Setup(r => r.GetAll(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(new List<User>()));

        var query = new GetUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().BeEmpty();
        result.Total.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    // Page < 1 girildiğinde otomatik olarak Page = 1 kabul edilmeli.
    [Fact]
    public async Task Handle_WithInvalidPage_ShouldDefaultToPageOne()
    {
        // Arrange
        var users = MakeUsers(2);
        _userRepo.Setup(r => r.GetAll(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(users));

        var query = new GetUsersQuery { Page = -5, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Page.Should().Be(1);
        result.Data.Should().HaveCount(2);
    }

    // IsActiveOnly=true ile GetAll çağrıldığında true parametre iletilmeli.
    [Fact]
    public async Task Handle_WithIsActiveOnlyTrue_ShouldPassFilterToRepository()
    {
        // Arrange
        _userRepo.Setup(r => r.GetAll(true))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(MakeUsers(2)));

        var query = new GetUsersQuery { IsActiveOnly = true };

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _userRepo.Verify(r => r.GetAll(true), Times.Once());
    }

    // Dönen DTO'ların email ve isim alanları entity ile eşleşmeli.
    [Fact]
    public async Task Handle_ShouldMapUserFieldsToDto()
    {
        // Arrange
        var user = User.Create("dto@example.com", "hash", "Mehmet", "Demir");
        _userRepo.Setup(r => r.GetAll(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable([user]));

        var query = new GetUsersQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var dto = result.Data.Single();
        dto.Email.Should().Be("dto@example.com");
        dto.FirstName.Should().Be("Mehmet");
        dto.LastName.Should().Be("Demir");
    }
}
