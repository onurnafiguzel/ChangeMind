using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorExerciseLibraryAndTrainingProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_TrainingPrograms_ProgramId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ProgramId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Reps",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "RestTimeSeconds",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TechniquesJson",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Exercises",
                newName: "MovementName");

            migrationBuilder.AddColumn<string>(
                name: "DailyProgramJson",
                table: "TrainingPrograms",
                type: "json",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProgramExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: false),
                    Reps = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Explanation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramExercises_TrainingPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "MovementName", "MuscleGroup" },
                values: new object[,]
                {
                    { new Guid("0c151438-80f4-4b5c-b1a4-6dd8e8698044"), "Ab Wheel Rollout", "Abs" },
                    { new Guid("0ee7e351-8d02-4bb6-96b1-0a95a76843c1"), "Hack Squat", "Legs" },
                    { new Guid("13b7de0e-b9fc-46a3-b9f3-04d03e7aa6f7"), "Machine Crunch", "Abs" },
                    { new Guid("148daebc-65b5-4553-b2dd-d9e1a038f2f0"), "Barbell Shrugs", "Traps" },
                    { new Guid("16d74c2d-674a-40af-8337-a04d916d80c7"), "Preacher Curl", "Biceps" },
                    { new Guid("1ac4feac-692d-459f-b279-fdf4d7e2ede9"), "Bent-over Barbell Row", "Back" },
                    { new Guid("1ea0bf87-2802-4914-9fbe-64512fb9355c"), "Hamstring Curl", "Legs" },
                    { new Guid("1ff50f27-2bb0-4ad9-b654-4fc984758e76"), "Smith Machine Squat", "Legs" },
                    { new Guid("21b84df6-c5e8-41be-9b69-927bd3d57f6a"), "Skull Crusher", "Triceps" },
                    { new Guid("29b48b13-9f8d-45c4-b22c-e7e5b8b8310a"), "Deadlift", "LowerBack" },
                    { new Guid("2eec9b7c-cc55-44af-9e9c-96dc9e95c636"), "Overhead Extension", "Triceps" },
                    { new Guid("372dbd88-076a-4d16-aa3f-bc58994789d9"), "Leg Extension", "Legs" },
                    { new Guid("3a97a80e-97a1-4b0f-b218-6a8df00acd17"), "Barbell Curl (Reverse)", "Forearms" },
                    { new Guid("3de5cad9-e51e-4f78-a4ab-2ad6f0e07b80"), "Rope Pushdown", "Triceps" },
                    { new Guid("4c78fb00-0a74-4e48-a12d-f205060776a8"), "Landmine Rotation", "Obliques" },
                    { new Guid("4e4dbae2-b69f-47a8-9c1d-f6e1b965fa67"), "Cable Curl", "Biceps" },
                    { new Guid("56632d26-3bc4-4391-b458-c45046fa826c"), "Cable Flyes", "Chest" },
                    { new Guid("593e3a95-29b5-4a19-a77a-aa911c49a1b3"), "Incline Dumbbell Press", "Chest" },
                    { new Guid("59f650fc-1aa3-4036-9a87-9fd2dbb6fefe"), "Bulgarian Split Squat", "Glutes" },
                    { new Guid("66509cb9-9a2c-45ce-a43e-8c68f7d5f390"), "Cable Crunch", "Abs" },
                    { new Guid("667bf71c-25c7-4dad-a05d-9182c02bf2b9"), "Arnold Press", "Shoulders" },
                    { new Guid("6b35163c-60bb-4301-9d5e-389a4d3d03d5"), "Dumbbell Curl", "Biceps" },
                    { new Guid("6f2903ff-0c1c-4958-a733-eaf4a1ab5b62"), "Machine Chest Press", "Chest" },
                    { new Guid("751213c5-bb92-4fcd-8940-5431b1d1f086"), "Hyperextension", "LowerBack" },
                    { new Guid("7a3c66b7-3a7f-4fd4-a61b-fc9dfad391a1"), "Seal Row", "Back" },
                    { new Guid("7bc1d7ce-5085-4dc4-86d8-d16fa6a29944"), "Underhand Lat Pulldown", "LatissimusDorsi" },
                    { new Guid("88d957dd-21b6-41de-bd95-526d8cd49574"), "Lat Pulldown", "Back" },
                    { new Guid("8dc4d7f5-ce13-4bfa-8920-8ed2efa117f8"), "Calf Raise", "Calves" },
                    { new Guid("8fcd6ffc-df27-4851-8a6c-293f864958ae"), "Tricep Dips", "Triceps" },
                    { new Guid("95bcb737-756d-4ba8-9aa1-673bcfc967d3"), "Lateral Raise", "Shoulders" },
                    { new Guid("a47cfde3-c00a-47af-8ec3-2689f731ad91"), "Overhead Press", "Shoulders" },
                    { new Guid("a5085e09-6992-404a-824c-dbb6878651b8"), "T-Bar Row", "Back" },
                    { new Guid("aa0f38fb-ddc8-4605-9d16-e77220c9af61"), "Cable Woodchop", "Obliques" },
                    { new Guid("b426b5c2-141b-4d55-abaa-44579cbaac54"), "Sissy Squat", "Quadriceps" },
                    { new Guid("b603b0ab-dd3b-4f72-8a65-672089844fa3"), "Hip Thrust", "Glutes" },
                    { new Guid("b74aa40e-7adb-49b0-aa84-05694f95fd5b"), "Leg Sled", "Legs" },
                    { new Guid("bd4b82b8-dc59-4e4e-bc06-1f97288a89f9"), "Chest-Supported Row", "Back" },
                    { new Guid("bd4f7900-6cfc-4b61-a419-5c7e33499bc8"), "Smith Machine Press", "Chest" },
                    { new Guid("c1bca2b3-d0fa-4cb6-970d-d376d6222458"), "Glute-Focused Leg Press", "Glutes" },
                    { new Guid("c22bc5da-b7ce-4a17-8deb-5fa173d755ae"), "Dumbbell Shrugs", "Traps" },
                    { new Guid("c5f6e156-f62b-40e0-a25a-131c8e500e3d"), "Front Squat", "Quadriceps" },
                    { new Guid("c7d73375-3072-4c80-a1ce-402d034506aa"), "Machine Shoulder Press", "Shoulders" },
                    { new Guid("ce2ae155-c43f-4475-bba1-a244482cc68e"), "Pull-ups", "Back" },
                    { new Guid("d1d4971e-8c87-4b7e-bbc4-c99e46cc58f2"), "Barbell Back Squat", "Legs" },
                    { new Guid("d400f482-2131-4460-adac-9fca975176bc"), "Wrist Curls", "Forearms" },
                    { new Guid("da1eabca-53d2-4484-b1a0-a560c8d84a6f"), "Barbell Curl", "Biceps" },
                    { new Guid("dd214085-df3b-40ac-bee9-ab3306137159"), "Barbell Bench Press", "Chest" },
                    { new Guid("e4b0cbfd-337d-4cf4-9af7-508f21558fa4"), "Machine Calf Raise", "Calves" },
                    { new Guid("eb81162b-b472-42e8-825e-eb222d9dc12f"), "Good Morning", "Hamstrings" },
                    { new Guid("ef1472d1-5d87-44b3-a928-681cd8abf5fa"), "Romanian Deadlift", "Hamstrings" },
                    { new Guid("efd20db2-bf3e-4ccd-91ea-8c30acad93b4"), "Decline Bench Press", "Chest" },
                    { new Guid("f3cf8da8-6d37-49f2-81a5-c8ebc9fa27cc"), "Reverse Pec Deck", "Shoulders" },
                    { new Guid("f4d36d2a-c45c-4caa-8447-c05d029a0f95"), "Leg Press", "Legs" },
                    { new Guid("facb100c-fdd9-4fe2-bde5-2d786cffdb50"), "Wide Grip Lat Pulldown", "LatissimusDorsi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MovementName",
                table: "Exercises",
                column: "MovementName");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ExerciseId",
                table: "ProgramExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ProgramId",
                table: "ProgramExercises",
                column: "ProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramExercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_MovementName",
                table: "Exercises");

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("0c151438-80f4-4b5c-b1a4-6dd8e8698044"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("0ee7e351-8d02-4bb6-96b1-0a95a76843c1"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("13b7de0e-b9fc-46a3-b9f3-04d03e7aa6f7"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("148daebc-65b5-4553-b2dd-d9e1a038f2f0"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("16d74c2d-674a-40af-8337-a04d916d80c7"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("1ac4feac-692d-459f-b279-fdf4d7e2ede9"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("1ea0bf87-2802-4914-9fbe-64512fb9355c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("1ff50f27-2bb0-4ad9-b654-4fc984758e76"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("21b84df6-c5e8-41be-9b69-927bd3d57f6a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("29b48b13-9f8d-45c4-b22c-e7e5b8b8310a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("2eec9b7c-cc55-44af-9e9c-96dc9e95c636"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("372dbd88-076a-4d16-aa3f-bc58994789d9"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("3a97a80e-97a1-4b0f-b218-6a8df00acd17"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("3de5cad9-e51e-4f78-a4ab-2ad6f0e07b80"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("4c78fb00-0a74-4e48-a12d-f205060776a8"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("4e4dbae2-b69f-47a8-9c1d-f6e1b965fa67"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("56632d26-3bc4-4391-b458-c45046fa826c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("593e3a95-29b5-4a19-a77a-aa911c49a1b3"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("59f650fc-1aa3-4036-9a87-9fd2dbb6fefe"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("66509cb9-9a2c-45ce-a43e-8c68f7d5f390"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("667bf71c-25c7-4dad-a05d-9182c02bf2b9"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("6b35163c-60bb-4301-9d5e-389a4d3d03d5"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("6f2903ff-0c1c-4958-a733-eaf4a1ab5b62"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("751213c5-bb92-4fcd-8940-5431b1d1f086"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("7a3c66b7-3a7f-4fd4-a61b-fc9dfad391a1"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("7bc1d7ce-5085-4dc4-86d8-d16fa6a29944"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("88d957dd-21b6-41de-bd95-526d8cd49574"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("8dc4d7f5-ce13-4bfa-8920-8ed2efa117f8"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("8fcd6ffc-df27-4851-8a6c-293f864958ae"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("95bcb737-756d-4ba8-9aa1-673bcfc967d3"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a47cfde3-c00a-47af-8ec3-2689f731ad91"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a5085e09-6992-404a-824c-dbb6878651b8"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("aa0f38fb-ddc8-4605-9d16-e77220c9af61"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("b426b5c2-141b-4d55-abaa-44579cbaac54"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("b603b0ab-dd3b-4f72-8a65-672089844fa3"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("b74aa40e-7adb-49b0-aa84-05694f95fd5b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("bd4b82b8-dc59-4e4e-bc06-1f97288a89f9"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("bd4f7900-6cfc-4b61-a419-5c7e33499bc8"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c1bca2b3-d0fa-4cb6-970d-d376d6222458"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c22bc5da-b7ce-4a17-8deb-5fa173d755ae"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c5f6e156-f62b-40e0-a25a-131c8e500e3d"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c7d73375-3072-4c80-a1ce-402d034506aa"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("ce2ae155-c43f-4475-bba1-a244482cc68e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("d1d4971e-8c87-4b7e-bbc4-c99e46cc58f2"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("d400f482-2131-4460-adac-9fca975176bc"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("da1eabca-53d2-4484-b1a0-a560c8d84a6f"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("dd214085-df3b-40ac-bee9-ab3306137159"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("e4b0cbfd-337d-4cf4-9af7-508f21558fa4"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("eb81162b-b472-42e8-825e-eb222d9dc12f"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("ef1472d1-5d87-44b3-a928-681cd8abf5fa"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("efd20db2-bf3e-4ccd-91ea-8c30acad93b4"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("f3cf8da8-6d37-49f2-81a5-c8ebc9fa27cc"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("f4d36d2a-c45c-4caa-8447-c05d029a0f95"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("facb100c-fdd9-4fe2-bde5-2d786cffdb50"));

            migrationBuilder.DropColumn(
                name: "DailyProgramJson",
                table: "TrainingPrograms");

            migrationBuilder.RenameColumn(
                name: "MovementName",
                table: "Exercises",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Exercises",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProgramId",
                table: "Exercises",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Reps",
                table: "Exercises",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestTimeSeconds",
                table: "Exercises",
                type: "integer",
                nullable: true,
                defaultValue: 60);

            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "Exercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TechniquesJson",
                table: "Exercises",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ProgramId",
                table: "Exercises",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_TrainingPrograms_ProgramId",
                table: "Exercises",
                column: "ProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
