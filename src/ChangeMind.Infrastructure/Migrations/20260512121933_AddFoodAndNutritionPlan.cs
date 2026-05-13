using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodAndNutritionPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CaloriesPer100g = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    ProteinPer100g = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    CarbsPer100g = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    FatPer100g = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutritionPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoachId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DailyPlanJson = table.Column<string>(type: "json", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionPlans_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NutritionPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "CaloriesPer100g", "CarbsPer100g", "CreatedAt", "FatPer100g", "IsActive", "Name", "ProteinPer100g", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-0000000f0001"), 165m, 0.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3.6m, true, "Tavuk Göğsü", 31.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0002"), 217m, 0.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11.8m, true, "Dana Bonfile", 26.1m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0003"), 135m, 0.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1.0m, true, "Hindi Göğsü", 30.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0004"), 208m, 0.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13.0m, true, "Somon", 20.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0005"), 125m, 0.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2.9m, true, "Levrek", 23.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0006"), 132m, 0.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1.0m, true, "Ton Balığı (konserve)", 28.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0007"), 155m, 1.1m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11.0m, true, "Yumurta (bütün)", 13.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0008"), 52m, 0.7m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Yumurta Akı", 11.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0009"), 59m, 3.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.4m, true, "Süzme Yoğurt", 10.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f000a"), 61m, 4.7m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3.3m, true, "Yoğurt (tam yağlı)", 3.5m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f000b"), 98m, 3.4m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4.3m, true, "Lor Peyniri", 11.1m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f000c"), 264m, 2.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21.0m, true, "Beyaz Peynir", 17.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f000d"), 61m, 4.8m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3.3m, true, "Süt (tam yağlı)", 3.2m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f000e"), 41m, 4.5m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1.0m, true, "Kefir", 3.3m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f000f"), 380m, 7.5m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3.5m, true, "Whey Protein Tozu", 80.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0010"), 130m, 28.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.3m, true, "Pirinç Pilavı", 2.7m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0011"), 123m, 25.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.9m, true, "Esmer Pirinç", 2.6m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0012"), 83m, 18.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Bulgur Pilavı", 3.1m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0013"), 389m, 66.3m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6.9m, true, "Yulaf Ezmesi", 16.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0014"), 120m, 21.3m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1.9m, true, "Kinoa", 4.4m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0015"), 131m, 25.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1.1m, true, "Makarna (haşlanmış)", 5.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0016"), 247m, 41.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3.4m, true, "Tam Buğday Ekmeği", 13.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0017"), 265m, 49.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3.2m, true, "Beyaz Ekmek", 9.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0018"), 86m, 20.1m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.1m, true, "Tatlı Patates", 1.6m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0019"), 77m, 17.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.1m, true, "Patates", 2.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f001a"), 116m, 20.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.4m, true, "Yeşil Mercimek", 9.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f001b"), 116m, 20.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.4m, true, "Kırmızı Mercimek", 9.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f001c"), 164m, 27.4m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2.6m, true, "Nohut (haşlanmış)", 8.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f001d"), 127m, 22.8m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.5m, true, "Kuru Fasulye", 8.7m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0020"), 34m, 6.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.4m, true, "Brokoli", 2.8m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0021"), 23m, 3.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.4m, true, "Ispanak", 2.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0022"), 18m, 3.9m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Domates", 0.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0023"), 16m, 3.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.1m, true, "Salatalık", 0.7m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0024"), 41m, 9.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Havuç", 0.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0025"), 25m, 5.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.3m, true, "Karnabahar", 1.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0026"), 17m, 3.1m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.3m, true, "Kabak", 1.2m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0027"), 20m, 4.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Biber (yeşil)", 0.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0028"), 15m, 2.9m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Marul", 1.4m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0029"), 25m, 3.7m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.7m, true, "Roka", 2.6m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0030"), 89m, 22.8m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.3m, true, "Muz", 1.1m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0031"), 52m, 14.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Elma", 0.3m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0032"), 32m, 7.7m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.3m, true, "Çilek", 0.7m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0033"), 57m, 14.5m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.3m, true, "Yaban Mersini", 0.7m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0034"), 47m, 11.8m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.1m, true, "Portakal", 0.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0035"), 57m, 15.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.1m, true, "Armut", 0.4m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0036"), 69m, 18.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.2m, true, "Üzüm", 0.7m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0040"), 579m, 21.6m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 49.9m, true, "Badem", 21.2m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0041"), 654m, 13.7m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 65.2m, true, "Ceviz", 15.2m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0042"), 628m, 16.7m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 60.8m, true, "Fındık", 14.9m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0043"), 588m, 20.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 50.0m, true, "Fıstık Ezmesi", 25.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0044"), 884m, 0.0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 100.0m, true, "Zeytinyağı", 0.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0045"), 160m, 8.5m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14.7m, true, "Avokado", 2.0m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0046"), 115m, 6.3m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10.7m, true, "Zeytin (siyah)", 0.8m, null },
                    { new Guid("00000000-0000-0000-0000-0000000f0047"), 595m, 21.2m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 53.8m, true, "Tahin", 17.0m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_IsActive",
                table: "Foods",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Name",
                table: "Foods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPlans_CoachId",
                table: "NutritionPlans",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPlans_CreatedAt",
                table: "NutritionPlans",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPlans_IsActive",
                table: "NutritionPlans",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPlans_UserId",
                table: "NutritionPlans",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "NutritionPlans");
        }
    }
}
