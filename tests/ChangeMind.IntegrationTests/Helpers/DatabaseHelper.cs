namespace ChangeMind.IntegrationTests.Helpers;

using ChangeMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;

// DatabaseHelper: Migration çalıştırma ve test izolasyonu için yardımcı sınıf.
//
// Migration: Her test suite başlamadan önce bir kez çalışır.
//            Tablolar TestContainers veritabanına uygulanır.
//
// ResetAsync: Her test bittikten sonra tüm tabloları TRUNCATE eder (Respawn).
//             Bir testin verisi bir sonrakini etkilemez.
public static class DatabaseHelper
{
    private static Respawner? _respawner;

    public static async Task MigrateAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ChangeMindDbContext>();
        await db.Database.MigrateAsync();
    }

    // Respawn: belirtilen tabloları TRUNCATE eder.
    // Exercises tablosu seed data içerdiği için hariç tutulur.
    public static async Task InitializeRespawnerAsync(string connectionString)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter        = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            // Exercises: EF Core seed data (HasData migration) — Respawn bu tabloyu sıfırlarsa
            // seed kayıtları gitmez çünkü migration tekrar çalışmaz. Tabloyu koru.
            TablesToIgnore   =
            [
                new Respawn.Graph.Table("__EFMigrationsHistory"),
                new Respawn.Graph.Table("Exercises")
            ]
        });
    }

    public static async Task ResetAsync(string connectionString)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        await _respawner!.ResetAsync(conn);
    }
}
