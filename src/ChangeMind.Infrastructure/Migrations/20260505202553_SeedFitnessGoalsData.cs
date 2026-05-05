using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedFitnessGoalsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FitnessGoals",
                columns: new[] { "Id", "Name", "Description", "IsActive", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("f0000000-0000-0000-0000-000000000001"), "Muscle Gain", "Focus on building and increasing muscle mass through resistance training and proper nutrition.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000002"), "Fat Loss", "Aim to reduce body fat percentage through caloric deficit and consistent exercise.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000003"), "Strength", "Develop maximum strength and power through heavy resistance training.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000004"), "Endurance", "Build cardiovascular and muscular endurance for sustained physical activity.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000005"), "Flexibility", "Improve range of motion and flexibility through stretching and mobility work.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000006"), "General Fitness", "Maintain overall health and fitness with balanced training across all aspects.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000007"), "Weight Loss", "Lose weight through a combination of exercise and dietary changes.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000008"), "Toning", "Develop muscle definition and create a leaner appearance through targeted training.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000009"), "Athletic", "Train for athletic performance and sport-specific skills and fitness.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000009"));
        }
    }
}
