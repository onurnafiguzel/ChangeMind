using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFitnessGoalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FitnessGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessGoals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitnessGoals_IsActive",
                table: "FitnessGoals",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "UX_FitnessGoals_Name",
                table: "FitnessGoals",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FitnessGoals");
        }
    }
}
