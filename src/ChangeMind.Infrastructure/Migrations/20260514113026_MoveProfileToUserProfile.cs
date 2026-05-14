using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveProfileToUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_FitnessGoals_FitnessGoalId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FitnessGoalId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FitnessGoalId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FitnessLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Users");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompletedProfile",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FitnessGoalId = table.Column<Guid>(type: "uuid", nullable: true),
                    FitnessLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DailyWorkLifestyle = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    GymDaysPerWeek = table.Column<int>(type: "integer", nullable: true),
                    HealthConditions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    FoodAllergies = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    SupplementInterest = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    WantsSupplementSupport = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_FitnessGoals_FitnessGoalId",
                        column: x => x.FitnessGoalId,
                        principalTable: "FitnessGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_FitnessGoalId",
                table: "UserProfiles",
                column: "FitnessGoalId");

            migrationBuilder.CreateIndex(
                name: "UX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            // Profile data was on Users; columns dropped above. Force everyone to
            // re-complete their profile via POST /api/users/{id}/complete-profile.
            migrationBuilder.Sql(@"UPDATE ""Users"" SET ""IsCompletedProfile"" = false;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompletedProfile",
                table: "Users",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "FitnessGoalId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FitnessLevel",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "Users",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Users",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FitnessGoalId",
                table: "Users",
                column: "FitnessGoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_FitnessGoals_FitnessGoalId",
                table: "Users",
                column: "FitnessGoalId",
                principalTable: "FitnessGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
