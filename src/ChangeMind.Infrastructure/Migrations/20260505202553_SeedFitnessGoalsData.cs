using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedFitnessGoalsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FitnessGoals",
                columns: new[] { "Id", "Name", "Description", "IsActive", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("f0000000-0000-0000-0000-000000000001"), "Kas Kazanımı", "Direnç antrenmanı ve doğru beslenme ile kas kütlesini artırmaya odaklan.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000002"), "Yağ Kaybı", "Kalori açığı ve düzenli egzersizle vücut yağ oranını düşürmeyi hedefle.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000003"), "Kuvvet", "Ağır direnç antrenmanı ile maksimum kuvvet ve güç geliştir.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000004"), "Dayanıklılık", "Sürdürülebilir fiziksel aktivite için kardiyovasküler ve kas dayanıklılığı geliştir.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000005"), "Esneklik", "Esneme ve mobilite çalışmaları ile hareket açıklığını ve esnekliği artır.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000006"), "Genel Fitness", "Tüm yönleri kapsayan dengeli bir antrenmanla genel sağlığını ve formunu koru.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000007"), "Kilo Verme", "Egzersiz ve beslenme değişiklikleriyle kilo ver.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000008"), "Sıkılaşma", "Hedefe yönelik antrenmanla kas tanımı kazan ve daha sıkı bir görünüm elde et.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000009"), "Atletik Performans", "Atletik performans ve spora özgü beceriler için antrenman yap.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("f0000000-0000-0000-0000-000000000010"), "Koça bırakıyorum", "Hedefinizi koçunuz sizin için belirlesin.", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "FitnessGoals",
                keyColumn: "Id",
                keyValue: new Guid("f0000000-0000-0000-0000-000000000010"));
        }
    }
}
