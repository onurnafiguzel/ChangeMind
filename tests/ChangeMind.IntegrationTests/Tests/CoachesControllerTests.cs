namespace ChangeMind.IntegrationTests.Tests;

using System.Net;
using System.Net.Http.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.IntegrationTests.Fixtures;
using ChangeMind.IntegrationTests.Helpers;
using FluentAssertions;

public class CoachesControllerTests(PostgreSqlFixture db)
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

    // CreateCoachCommand requires an existing user with the same email.
    // The handler copies firstName, lastName, and passwordHash from the user entity.
    private async Task<string> CreateUserAndReturnEmailAsync(string email = "coach.user@example.com")
    {
        var userRequest = new
        {
            Email    = email,
            Password = "Test1234!"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/users", userRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Complete profile so FirstName/LastName are populated on the user
        var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();
        await _client.PostAsJsonAsync($"/api/users/{userId}/complete-profile", new
        {
            FirstName = "Koç",
            LastName  = "Deneme"
        });

        return email;
    }

    // -----------------------------------------------------------------------
    // GET /api/coaches
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetCoaches_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/coaches");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCoaches_EmptyDatabase_ShouldReturnEmptyList()
    {
        var response = await _client.GetAsync("/api/coaches?isActiveOnly=false");
        var body     = await response.Content.ReadFromJsonAsync<PagedResult<CoachDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Data.Should().BeEmpty();
        body.Total.Should().Be(0);
    }

    // -----------------------------------------------------------------------
    // POST /api/coaches
    // -----------------------------------------------------------------------

    [Fact]
    public async Task CreateCoach_ValidRequest_ShouldReturn201()
    {
        var email = await CreateUserAndReturnEmailAsync("create.coach@example.com");

        var response = await _client.PostAsJsonAsync("/api/coaches", new
        {
            Email          = email,
            Specialization = "Strength"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateCoach_ValidRequest_ShouldAppearInList()
    {
        var email = await CreateUserAndReturnEmailAsync("listed.coach@example.com");

        await _client.PostAsJsonAsync("/api/coaches", new
        {
            Email          = email,
            Specialization = "Cardio"
        });

        var listResponse = await _client.GetAsync("/api/coaches?isActiveOnly=true");
        var body         = await listResponse.Content.ReadFromJsonAsync<PagedResult<CoachDto>>();

        body!.Data.Should().ContainSingle(c => c.Email == email);
    }

    [Fact]
    public async Task CreateCoach_DuplicateEmail_ShouldReturn409()
    {
        var email = await CreateUserAndReturnEmailAsync("dup.coach@example.com");

        await _client.PostAsJsonAsync("/api/coaches", new { Email = email });
        var response = await _client.PostAsJsonAsync("/api/coaches", new { Email = email });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task CreateCoach_NonExistingUserEmail_ShouldReturn404()
    {
        var response = await _client.PostAsJsonAsync("/api/coaches", new
        {
            Email = "nonexistent@example.com"
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateCoach_MissingEmail_ShouldReturn400()
    {
        var response = await _client.PostAsJsonAsync("/api/coaches", new
        {
            Specialization = "Strength"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // -----------------------------------------------------------------------
    // GET /api/coaches/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetCoachById_ExistingId_ShouldReturn200WithCoach()
    {
        var email = await CreateUserAndReturnEmailAsync("getbyid.coach@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/coaches", new
        {
            Email          = email,
            Specialization = "Bodybuilding"
        });
        var coachId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var response = await _client.GetAsync($"/api/coaches/{coachId}");
        var body     = await response.Content.ReadFromJsonAsync<CoachDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetCoachById_NonExistingId_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/api/coaches/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -----------------------------------------------------------------------
    // PUT /api/coaches/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task UpdateCoach_ExistingId_ShouldReturn204()
    {
        var email = await CreateUserAndReturnEmailAsync("update.coach@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/coaches", new { Email = email });
        var coachId        = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var updateRequest = new
        {
            FirstName      = "Güncellendi",
            LastName       = "Koç",
            Specialization = "CrossFit"
        };

        var response = await _client.PutAsJsonAsync($"/api/coaches/{coachId}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateCoach_NonExistingId_ShouldReturn404()
    {
        var updateRequest = new { FirstName = "Test", LastName = "Coach" };

        var response = await _client.PutAsJsonAsync($"/api/coaches/{Guid.NewGuid()}", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -----------------------------------------------------------------------
    // DELETE /api/coaches/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task DeleteCoach_ExistingId_ShouldReturn204()
    {
        var email = await CreateUserAndReturnEmailAsync("delete.coach@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/coaches", new { Email = email });
        var coachId        = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var response = await _client.DeleteAsync($"/api/coaches/{coachId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteCoach_ShouldDisappearFromActiveList()
    {
        var email = await CreateUserAndReturnEmailAsync("softdelete.coach@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/coaches", new { Email = email });
        var coachId        = await createResponse.Content.ReadFromJsonAsync<Guid>();

        await _client.DeleteAsync($"/api/coaches/{coachId}");

        var listResponse = await _client.GetAsync("/api/coaches?isActiveOnly=true");
        var body         = await listResponse.Content.ReadFromJsonAsync<PagedResult<CoachDto>>();

        body!.Data.Should().NotContain(c => c.Id == coachId);
    }

    // -----------------------------------------------------------------------
    // POST /api/coaches/{id}/change-password
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ChangeCoachPassword_CorrectCurrentPassword_ShouldReturn204()
    {
        var email = await CreateUserAndReturnEmailAsync("changepw.coach@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/coaches", new { Email = email });
        var coachId        = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var changeRequest = new
        {
            CurrentPassword = "Test1234!",
            NewPassword     = "NewCoachPass456!"
        };

        var response = await _client.PostAsJsonAsync($"/api/coaches/{coachId}/change-password", changeRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ChangeCoachPassword_WrongCurrentPassword_ShouldReturn401()
    {
        var email = await CreateUserAndReturnEmailAsync("wrongpw.coach@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/coaches", new { Email = email });
        var coachId        = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var changeRequest = new
        {
            CurrentPassword = "WrongPassword!",
            NewPassword     = "NewCoachPass456!"
        };

        var response = await _client.PostAsJsonAsync($"/api/coaches/{coachId}/change-password", changeRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
