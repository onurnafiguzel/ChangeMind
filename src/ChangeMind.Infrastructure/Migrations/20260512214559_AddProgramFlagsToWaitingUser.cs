using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramFlagsToWaitingUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasNutritionPlan",
                table: "WaitingUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasTrainingProgram",
                table: "WaitingUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            // Backfill flags from existing active TrainingPrograms / NutritionPlans
            migrationBuilder.Sql(@"
                UPDATE ""WaitingUsers"" w
                SET ""HasTrainingProgram"" = EXISTS (
                    SELECT 1 FROM ""TrainingPrograms"" tp
                    WHERE tp.""UserId"" = w.""UserId"" AND tp.""IsActive"" = true
                );
                UPDATE ""WaitingUsers"" w
                SET ""HasNutritionPlan"" = EXISTS (
                    SELECT 1 FROM ""NutritionPlans"" np
                    WHERE np.""UserId"" = w.""UserId"" AND np.""IsActive"" = true
                );
                UPDATE ""WaitingUsers""
                SET ""IsWaitingForAssignment"" = NOT (""HasTrainingProgram"" AND ""HasNutritionPlan"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasNutritionPlan",
                table: "WaitingUsers");

            migrationBuilder.DropColumn(
                name: "HasTrainingProgram",
                table: "WaitingUsers");
        }
    }
}
