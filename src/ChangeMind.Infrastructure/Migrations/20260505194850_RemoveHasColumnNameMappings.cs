using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHasColumnNameMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Exercise.MovementName özelliği EF model snapshot'ta "MovementName" sütununa
            // bağlanıyor; ancak 20260414133100_exercise-library-crud migration'ı DB sütununu
            // "Name" olarak yeniden adlandırmıştı. Burada DB tarafında düzeltiyoruz.
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Exercises",
                newName: "MovementName");

            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "Exercises",
                newName: "VideoLink");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovementName",
                table: "Exercises",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "VideoLink",
                table: "Exercises",
                newName: "VideoUrl");
        }
    }
}
