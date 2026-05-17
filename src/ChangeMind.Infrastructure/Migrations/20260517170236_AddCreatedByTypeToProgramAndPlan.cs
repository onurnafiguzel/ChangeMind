using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByTypeToProgramAndPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NutritionPlans_Coaches_CoachId",
                table: "NutritionPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPrograms_Coaches_CoachId",
                table: "TrainingPrograms");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoachId",
                table: "TrainingPrograms",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByType",
                table: "TrainingPrograms",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Coach");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoachId",
                table: "NutritionPlans",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByType",
                table: "NutritionPlans",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Coach");

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionPlans_Coaches_CoachId",
                table: "NutritionPlans",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPrograms_Coaches_CoachId",
                table: "TrainingPrograms",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NutritionPlans_Coaches_CoachId",
                table: "NutritionPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPrograms_Coaches_CoachId",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "CreatedByType",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "CreatedByType",
                table: "NutritionPlans");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoachId",
                table: "TrainingPrograms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CoachId",
                table: "NutritionPlans",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionPlans_Coaches_CoachId",
                table: "NutritionPlans",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPrograms_Coaches_CoachId",
                table: "TrainingPrograms",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
