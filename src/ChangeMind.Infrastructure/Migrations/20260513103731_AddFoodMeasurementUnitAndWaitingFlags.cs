using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodMeasurementUnitAndWaitingFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProteinPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "FatPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "CarbsPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "CaloriesPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "CaloriesPerPiece",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CarbsPerPiece",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FatPerPiece",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GramsPerPiece",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PieceLabel",
                table: "Foods",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProteinPerPiece",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Foods",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Grams");

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0001"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0002"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0003"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0004"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0005"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0006"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0007"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 78m, null, 0.5m, null, 5.5m, 50m, "1 adet (orta)", null, 6.5m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0008"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 17m, null, 0.2m, null, 0.1m, 33m, "1 adet", null, 3.6m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0009"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f000a"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f000b"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f000c"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f000d"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f000e"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f000f"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0010"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0011"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0012"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0013"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0014"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0015"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0016"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 74m, null, 12.3m, null, 1.0m, 30m, "1 dilim", null, 3.9m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0017"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 80m, null, 14.7m, null, 1.0m, 30m, "1 dilim", null, 2.7m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0018"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0019"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f001a"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f001b"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f001c"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f001d"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0020"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0021"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0022"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0023"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0024"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0025"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0026"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0027"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0028"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0029"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0030"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 105m, null, 27.0m, null, 0.4m, 118m, "1 adet (orta)", null, 1.3m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0031"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 78m, null, 21.0m, null, 0.3m, 150m, "1 adet (orta)", null, 0.4m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0032"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0033"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0034"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 65m, null, 16.3m, null, 0.2m, 140m, "1 adet (orta)", null, 1.3m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0035"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 102m, null, 27.0m, null, 0.2m, 180m, "1 adet (orta)", null, 0.6m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0036"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0040"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 144m, null, 5.4m, null, 12.5m, 25m, "1 avuç (~25g)", null, 5.3m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0041"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 33m, null, 0.7m, null, 3.3m, 5m, "1 adet (çekirdek)", null, 0.8m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0042"),
                columns: new[] { "CaloriesPer100g", "CaloriesPerPiece", "CarbsPer100g", "CarbsPerPiece", "FatPer100g", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPer100g", "ProteinPerPiece", "Unit" },
                values: new object[] { null, 157m, null, 4.2m, null, 15.2m, 25m, "1 avuç (~25g)", null, 3.7m, "Piece" });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0043"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0044"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0045"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0046"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0047"),
                columns: new[] { "CaloriesPerPiece", "CarbsPerPiece", "FatPerPiece", "GramsPerPiece", "PieceLabel", "ProteinPerPiece" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Unit",
                table: "Foods",
                column: "Unit");

            // Backfill WaitingUsers flags based on existing active programs/plans
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
            migrationBuilder.DropIndex(
                name: "IX_Foods_Unit",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CaloriesPerPiece",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CarbsPerPiece",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "FatPerPiece",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "GramsPerPiece",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "PieceLabel",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ProteinPerPiece",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Foods");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProteinPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FatPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CarbsPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CaloriesPer100g",
                table: "Foods",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0007"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 155m, 1.1m, 11.0m, 13.0m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0008"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 52m, 0.7m, 0.2m, 11.0m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0016"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 247m, 41.0m, 3.4m, 13.0m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0017"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 265m, 49.0m, 3.2m, 9.0m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0030"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 89m, 22.8m, 0.3m, 1.1m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0031"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 52m, 14.0m, 0.2m, 0.3m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0034"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 47m, 11.8m, 0.1m, 0.9m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0035"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 57m, 15.0m, 0.1m, 0.4m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0040"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 579m, 21.6m, 49.9m, 21.2m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0041"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 654m, 13.7m, 65.2m, 15.2m });

            migrationBuilder.UpdateData(
                table: "Foods",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-0000000f0042"),
                columns: new[] { "CaloriesPer100g", "CarbsPer100g", "FatPer100g", "ProteinPer100g" },
                values: new object[] { 628m, 16.7m, 60.8m, 14.9m });
        }
    }
}
