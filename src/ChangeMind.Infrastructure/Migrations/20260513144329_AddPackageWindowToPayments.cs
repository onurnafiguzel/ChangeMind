using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageWindowToPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PackageEndDate",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PackageStartDate",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PackageEndDate",
                table: "Payments",
                column: "PackageEndDate");

            // Backfill: derive package window from existing completed payments
            migrationBuilder.Sql(@"
                UPDATE ""Payments"" p
                SET ""PackageStartDate"" = p.""CompletedAt"",
                    ""PackageEndDate""   = p.""CompletedAt"" + (pk.""DurationDays"" * INTERVAL '1 day')
                FROM ""Packages"" pk
                WHERE p.""PackageId"" = pk.""Id""
                  AND p.""Status"" = 'Completed'
                  AND p.""CompletedAt"" IS NOT NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_PackageEndDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PackageEndDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PackageStartDate",
                table: "Payments");
        }
    }
}
