namespace ChangeMind.IntegrationTests.Tests;

using System.Net;
using System.Net.Http.Json;
using ChangeMind.Application.DTOs;
using ChangeMind.IntegrationTests.Fixtures;
using ChangeMind.IntegrationTests.Helpers;
using FluentAssertions;

public class ExercisesControllerTests(PostgreSqlFixture db)
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
    // GET /api/exercises
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetExercises_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/exercises");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // Exercises tablosu EF Core seed data ile 57 egzersiz içeriyor
    [Fact]
    public async Task GetExercises_ShouldReturnSeededExercises()
    {
        var response = await _client.GetAsync("/api/exercises?isActiveOnly=false&pageSize=100");
        var body     = await response.Content.ReadFromJsonAsync<PagedResult<ExerciseDto>>();

        body!.Total.Should().BeGreaterThan(0);
        body.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetExercises_FilterByMuscleGroup_ShouldReturnFiltered()
    {
        var response = await _client.GetAsync("/api/exercises?muscleGroup=Chest&isActiveOnly=false");
        var body     = await response.Content.ReadFromJsonAsync<PagedResult<ExerciseDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Data.Should().AllSatisfy(e => e.MuscleGroup.Should().Be("Chest"));
    }

    [Fact]
    public async Task GetExercises_FilterByDifficultyLevel_ShouldReturnFiltered()
    {
        var response = await _client.GetAsync("/api/exercises?difficultyLevel=Beginner&isActiveOnly=false");
        var body     = await response.Content.ReadFromJsonAsync<PagedResult<ExerciseDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Data.Should().AllSatisfy(e => e.DifficultyLevel.Should().Be("Beginner"));
    }

    // -----------------------------------------------------------------------
    // POST /api/exercises
    // -----------------------------------------------------------------------

    [Fact]
    public async Task CreateExercise_ValidRequest_ShouldReturn201()
    {
        var request = new
        {
            Name           = "Custom Squat",
            MuscleGroup    = "Legs",
            DifficultyLevel = "Beginner"
        };

        var response = await _client.PostAsJsonAsync("/api/exercises", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateExercise_ValidRequest_ShouldBeRetrievableById()
    {
        var request = new
        {
            Name            = "Test Push Up",
            MuscleGroup     = "Chest",
            DifficultyLevel = "Intermediate",
            Description     = "Göğüs egzersizi"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/exercises", request);
        var createdId      = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var getResponse = await _client.GetAsync($"/api/exercises/{createdId}");
        var body        = await getResponse.Content.ReadFromJsonAsync<ExerciseDto>();

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Name.Should().Be("Test Push Up");
        body.Description.Should().Be("Göğüs egzersizi");
        body.MuscleGroup.Should().Be("Chest");
    }

    [Fact]
    public async Task CreateExercise_InvalidMuscleGroup_ShouldReturn400()
    {
        var request = new
        {
            Name            = "Test",
            MuscleGroup     = "GeçersizKasGrubu",
            DifficultyLevel = "Beginner"
        };

        var response = await _client.PostAsJsonAsync("/api/exercises", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateExercise_MissingName_ShouldReturn400()
    {
        var request = new
        {
            MuscleGroup     = "Legs",
            DifficultyLevel = "Beginner"
        };

        var response = await _client.PostAsJsonAsync("/api/exercises", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // -----------------------------------------------------------------------
    // GET /api/exercises/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetExerciseById_NonExistingId_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/api/exercises/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -----------------------------------------------------------------------
    // DELETE /api/exercises/{id}
    // -----------------------------------------------------------------------

    [Fact]
    public async Task DeleteExercise_ExistingId_ShouldReturn204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/exercises", new
        {
            Name            = "Silinecek Egzersiz",
            MuscleGroup     = "Back",
            DifficultyLevel = "Advanced"
        });
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var deleteResponse = await _client.DeleteAsync($"/api/exercises/{id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
