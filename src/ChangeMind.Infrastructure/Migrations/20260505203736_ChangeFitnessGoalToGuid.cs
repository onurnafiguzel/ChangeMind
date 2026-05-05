using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFitnessGoalToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FitnessGoal",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "FitnessGoalId",
                table: "Users",
                type: "uuid",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_FitnessGoals_FitnessGoalId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FitnessGoalId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FitnessGoalId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "FitnessGoal",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
