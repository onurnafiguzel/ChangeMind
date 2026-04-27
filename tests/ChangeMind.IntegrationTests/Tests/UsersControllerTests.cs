namespace ChangeMind.IntegrationTests.Tests;

using System.Net;
using System.Net.Http.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.IntegrationTests.Fixtures;
using ChangeMind.IntegrationTests.Helpers;
using FluentAssertions;

public class UsersControllerTests(PostgreSqlFixture db)
    : IClassFixture<PostgreSqlFixture>, IAsyncLifetime
{
    private HttpClient _client = null!;
    private WebAppFactory _factory = null!;

    public async Task InitializeAsync()
    {
        _factory = new WebAppFactory(db);
        _client  = _factory.CreateClient();

        await DatabaseHelper.MigrateAsync(_factory.Services);
        await DatabaseHelper.InitializeRespawnerAsync(db.ConnectionString);
    }

    public async Task DisposeAsync()
    {
        await DatabaseHelper.ResetAsync(db.ConnectionString);
        await _factory.DisposeAsync();
    }

    // -----------------------------------------------------------------------
    // GET /api/users
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetUsers_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUsers_EmptyDatabase_ShouldReturnEmptyList()
    {
        var response = await _client.GetAsync("/api/users?isActiveOnly=false");
        var body     = await response.Content.ReadFromJsonAsync<PagedResult<UserDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Data.Should().BeEmpty();
        body.Total.Should().Be(0);
    }

    // -----------------------------------------------------------------------
    // POST /api/users
    // -----------------------------------------------------------------------

    [Fact]
    public async Task CreateUser_ValidRequest_ShouldReturn201()
    {
        var request = new
        {
            Email    = "test@example.com",
            Password = "Test1234!"
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateUser_ValidRequest_ShouldAppearInList()
    {
        var request = new
        {
            Email    = "listed@example.com",
            Password = "Test1234!"
        };

        await _client.PostAsJsonAsync("/api/users", request);

        var listResponse = await _client.GetAsync("/api/users?isActiveOnly=true");
        var body         = await listResponse.Content.ReadFromJsonAsync<PagedResult<UserDto>>();

        body!.Data.Should().ContainSingle(u => u.Email == "listed@example.com");
    }

    [Fact]
    public async Task CreateUser_DuplicateEmail_ShouldReturn409()
    {
        var request = new { Email = "dup@example.com", Password = "Test1234!" };

        await _client.PostAsJsonAsync("/api/users", request);
        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task CreateUser_MissingEmail_ShouldReturn400()
    {
        var request = new { Password = "Test1234!" };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // -----------------------------------------------------------------------
    // GET /api/users/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetUserById_ExistingId_ShouldReturn200()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/users", new
        {
            Email    = "byid@example.com",
            Password = "Test1234!"
        });
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var response = await _client.GetAsync($"/api/users/{userId}");
        var body     = await response.Content.ReadFromJsonAsync<UserDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Email.Should().Be("byid@example.com");
    }

    [Fact]
    public async Task GetUserById_NonExistingId_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/api/users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -----------------------------------------------------------------------
    // POST /api/users/{id}/complete-profile
    // -----------------------------------------------------------------------

    [Fact]
    public async Task CompleteProfile_ValidRequest_ShouldReturn204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/users", new
        {
            Email    = "profile@example.com",
            Password = "Test1234!"
        });
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var profileRequest = new
        {
            FirstName   = "Ahmet",
            LastName    = "Yılmaz",
            Age         = 28,
            FitnessGoal = "WeightLoss"
        };

        var response = await _client.PostAsJsonAsync($"/api/users/{userId}/complete-profile", profileRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CompleteProfile_NonExistingUser_ShouldReturn404()
    {
        var profileRequest = new { FirstName = "Test", LastName = "User" };

        var response = await _client.PostAsJsonAsync($"/api/users/{Guid.NewGuid()}/complete-profile", profileRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -----------------------------------------------------------------------
    // PUT /api/users/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task UpdateUser_ExistingId_ShouldReturn204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/users", new
        {
            Email    = "update@example.com",
            Password = "Test1234!"
        });
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var updateRequest = new
        {
            FirstName = "Güncellendi",
            LastName  = "Kullanıcı"
        };

        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateUser_NonExistingId_ShouldReturn404()
    {
        var updateRequest = new { FirstName = "Test", LastName = "User" };

        var response = await _client.PutAsJsonAsync($"/api/users/{Guid.NewGuid()}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -----------------------------------------------------------------------
    // DELETE /api/users/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task DeleteUser_ExistingId_ShouldReturn204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/users", new
        {
            Email    = "delete@example.com",
            Password = "Test1234!"
        });
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var response = await _client.DeleteAsync($"/api/users/{userId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteUser_ShouldDisappearFromActiveList()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/users", new
        {
            Email    = "softdelete@example.com",
            Password = "Test1234!"
        });
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        await _client.DeleteAsync($"/api/users/{userId}");

        var listResponse = await _client.GetAsync("/api/users?isActiveOnly=true");
        var body         = await listResponse.Content.ReadFromJsonAsync<PagedResult<UserDto>>();

        body!.Data.Should().NotContain(u => u.Id == userId);
    }

    // -----------------------------------------------------------------------
    // POST /api/users/{id}/change-password
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ChangePassword_CorrectCurrentPassword_ShouldReturn204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/users", new
        {
            Email    = "changepw@example.com",
            Password = "OldPass123!"
        });
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var changeRequest = new
        {
            CurrentPassword = "OldPass123!",
            NewPassword     = "NewPass456!"
        };

        var response = await _client.PostAsJsonAsync($"/api/users/{userId}/change-password", changeRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ChangePassword_WrongCurrentPassword_ShouldReturn401()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/users", new
        {
            Email    = "wrongpw@example.com",
            Password = "OldPass123!"
        });
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var changeRequest = new
        {
            CurrentPassword = "WrongPassword!",
            NewPassword     = "NewPass456!"
        };

        var response = await _client.PostAsJsonAsync($"/api/users/{userId}/change-password", changeRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
