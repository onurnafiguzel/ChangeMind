namespace ChangeMind.UnitTests.Application;

using ChangeMind.Application.DTOs;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.UseCases.Users.Queries;
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

    private static List<UserDto> MakeUsers(int count)
        => Enumerable.Range(1, count)
            .Select(i => new UserDto
            {
                Id        = Guid.NewGuid(),
                Email     = $"user{i}@example.com",
                FirstName = $"Ad{i}",
                LastName  = $"Soyad{i}",
                IsActive  = true
            })
            .ToList();

    [Fact]
    public async Task Handle_WithExistingUsers_ShouldReturnPagedResult()
    {
        var users = MakeUsers(5);
        _userRepo.Setup(r => r.GetAllWithFitnessGoal(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(users));

        var query = new GetUsersQuery { Page = 1, PageSize = 10 };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(5);
        result.Total.Should().Be(5);
        result.Page.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        var users = MakeUsers(3);
        _userRepo.Setup(r => r.GetAllWithFitnessGoal(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(users));

        var query = new GetUsersQuery { Page = 2, PageSize = 2 };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Data.Should().HaveCount(1);
        result.Total.Should().Be(3);
        result.TotalPages.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WithNoUsers_ShouldReturnEmptyResult()
    {
        _userRepo.Setup(r => r.GetAllWithFitnessGoal(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(new List<UserDto>()));

        var query = new GetUsersQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Data.Should().BeEmpty();
        result.Total.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithInvalidPage_ShouldDefaultToPageOne()
    {
        var users = MakeUsers(2);
        _userRepo.Setup(r => r.GetAllWithFitnessGoal(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(users));

        var query = new GetUsersQuery { Page = -5, PageSize = 10 };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Page.Should().Be(1);
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithIsActiveOnlyTrue_ShouldPassFilterToRepository()
    {
        _userRepo.Setup(r => r.GetAllWithFitnessGoal(true))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable(MakeUsers(2)));

        var query = new GetUsersQuery { IsActiveOnly = true };

        await _handler.Handle(query, CancellationToken.None);

        _userRepo.Verify(r => r.GetAllWithFitnessGoal(true), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldMapUserFieldsToDto()
    {
        var user = new UserDto
        {
            Id        = Guid.NewGuid(),
            Email     = "dto@example.com",
            FirstName = "Mehmet",
            LastName  = "Demir"
        };
        _userRepo.Setup(r => r.GetAllWithFitnessGoal(It.IsAny<bool?>()))
                 .Returns(AsyncQueryHelper.BuildAsyncQueryable([user]));

        var query = new GetUsersQuery { Page = 1, PageSize = 10 };

        var result = await _handler.Handle(query, CancellationToken.None);

        var dto = result.Data.Single();
        dto.Email.Should().Be("dto@example.com");
        dto.FirstName.Should().Be("Mehmet");
        dto.LastName.Should().Be("Demir");
    }
}
