using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class exerciselibrarycrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovementName",
                table: "Exercises",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_MovementName",
                table: "Exercises",
                newName: "IX_Exercises_Name");

            migrationBuilder.AlterColumn<string>(
                name: "MuscleGroup",
                table: "Exercises",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Exercises",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyLevel",
                table: "Exercises",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Exercises",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Exercises",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Exercises",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010004"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010005"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010006"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020004"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020005"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020006"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030004"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030005"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040004"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050004"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000060001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000060002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070004"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070005"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070006"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070007"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000080001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000080002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000090001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000090002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000110001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000110002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120003"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000130001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000130002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000140001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000140002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000150001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000150002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000160001"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000160002"),
                columns: new[] { "CreatedAt", "Description", "DifficultyLevel", "IsActive", "UpdatedAt", "VideoUrl" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_IsActive",
                table: "Exercises",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exercises_IsActive",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "DifficultyLevel",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Exercises",
                newName: "MovementName");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_Name",
                table: "Exercises",
                newName: "IX_Exercises_MovementName");

            migrationBuilder.AlterColumn<string>(
                name: "MuscleGroup",
                table: "Exercises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
