using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBodyMeasurementsAndPersonalRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    WaistCm = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    ArmCm = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    LegCm = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    NeckCm = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    HipCm = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    BodyFatPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyMeasurements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Lift = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(6,2)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyMeasurements_UserId_RecordedAt",
                table: "BodyMeasurements",
                columns: new[] { "UserId", "RecordedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRecords_UserId_Lift",
                table: "PersonalRecords",
                columns: new[] { "UserId", "Lift" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyMeasurements");

            migrationBuilder.DropTable(
                name: "PersonalRecords");
        }
    }
}
