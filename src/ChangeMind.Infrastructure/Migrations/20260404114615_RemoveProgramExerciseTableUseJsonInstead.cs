using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProgramExerciseTableUseJsonInstead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramExercises");

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

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "MovementName", "MuscleGroup" },
                values: new object[,]
                {
                    { new Guid("098c21d8-a07f-4e94-a5c4-cbcab805e57a"), "Decline Bench Press", "Chest" },
                    { new Guid("0d9fc89e-d76c-47a4-88b3-9c411c08027c"), "Skull Crusher", "Triceps" },
                    { new Guid("1640c229-0e3c-4637-8770-4e1fd2500e65"), "T-Bar Row", "Back" },
                    { new Guid("18a106b5-44a0-48dd-94a5-0c548a6f96f0"), "Barbell Bench Press", "Chest" },
                    { new Guid("203fa475-9985-47fc-8a5f-deb44a617624"), "Bent-over Barbell Row", "Back" },
                    { new Guid("208ef74f-77a6-4abe-9e95-c117025861a9"), "Cable Crunch", "Abs" },
                    { new Guid("33252753-2ddd-4f1b-bb56-555346a70bdc"), "Cable Flyes", "Chest" },
                    { new Guid("33e0ca24-3b60-40c0-a8c8-793730797ff8"), "Landmine Rotation", "Obliques" },
                    { new Guid("355ac64f-15ff-48a0-8b24-ed3e389f341c"), "Incline Dumbbell Press", "Chest" },
                    { new Guid("35c6f8bb-5e71-45a5-9ad2-1689edbc20b2"), "Leg Sled", "Legs" },
                    { new Guid("3835c651-336b-4196-a1e8-5412ce6a2306"), "Machine Shoulder Press", "Shoulders" },
                    { new Guid("384f06df-df2e-4fa7-a19e-9fd40d6cc988"), "Chest-Supported Row", "Back" },
                    { new Guid("3a754d12-8c9d-438e-a79a-3a624e31bbd6"), "Glute-Focused Leg Press", "Glutes" },
                    { new Guid("3e83a9fe-1cf0-4be0-a110-d4e6ddf22bab"), "Deadlift", "LowerBack" },
                    { new Guid("40207d57-bbf1-44d8-a540-7a4f2154c41b"), "Bulgarian Split Squat", "Glutes" },
                    { new Guid("40f7d582-fcc5-4e11-a169-f506e3d71d8e"), "Barbell Back Squat", "Legs" },
                    { new Guid("42c707f8-277b-4638-bf9c-df54adf2c05d"), "Overhead Press", "Shoulders" },
                    { new Guid("448ea96b-6576-45ae-acf0-45758add7b91"), "Wrist Curls", "Forearms" },
                    { new Guid("4b3d4296-f921-44ef-a770-205443c424cf"), "Front Squat", "Quadriceps" },
                    { new Guid("4b4c5311-8a7d-4197-baec-b37ba3e910a2"), "Cable Curl", "Biceps" },
                    { new Guid("4efb25c3-3aa4-4430-87bd-184f1fbe2985"), "Dumbbell Curl", "Biceps" },
                    { new Guid("4f3a65cd-6f34-4c63-bcb1-8c3cd67dc62f"), "Smith Machine Squat", "Legs" },
                    { new Guid("52a67749-32e7-40bb-9d8e-e889cd107073"), "Machine Crunch", "Abs" },
                    { new Guid("54256f9f-ee01-420d-9174-1703d5f25202"), "Barbell Shrugs", "Traps" },
                    { new Guid("5edd3cf3-4a78-483f-935e-44123029c2ba"), "Leg Extension", "Legs" },
                    { new Guid("6366bd35-fe86-45d0-8563-cf127f4cbbc2"), "Hyperextension", "LowerBack" },
                    { new Guid("6c6985a3-53e3-43a7-b2b7-741cade921d1"), "Barbell Curl", "Biceps" },
                    { new Guid("6e8cd7a9-c344-4a49-b3f3-5ff78dae42e7"), "Dumbbell Shrugs", "Traps" },
                    { new Guid("6f6a9fae-c8fa-4fe7-8fa7-23066bf1eb26"), "Smith Machine Press", "Chest" },
                    { new Guid("6fef3bfc-6b21-4385-9156-361752a8249c"), "Lateral Raise", "Shoulders" },
                    { new Guid("72de0da4-a19c-427d-906d-caaeab63f49d"), "Hack Squat", "Legs" },
                    { new Guid("8a33a91b-9b07-4f97-9de8-15e7006d94ef"), "Pull-ups", "Back" },
                    { new Guid("9165606a-bd04-416f-a899-6f509733124b"), "Overhead Extension", "Triceps" },
                    { new Guid("9343b759-05f6-48a5-8990-4732d0b0e146"), "Preacher Curl", "Biceps" },
                    { new Guid("977edc78-e616-4b53-a82b-0deacf28f264"), "Calf Raise", "Calves" },
                    { new Guid("99e68b24-b09a-44d3-8de7-06eec94978de"), "Tricep Dips", "Triceps" },
                    { new Guid("9a1582de-8467-4e28-ac6d-d980f1f444c4"), "Machine Calf Raise", "Calves" },
                    { new Guid("9b2d3052-a6a4-41cf-9e26-8c98244ba3b0"), "Wide Grip Lat Pulldown", "LatissimusDorsi" },
                    { new Guid("9dbb1273-74fd-443b-8120-122010d40bfb"), "Good Morning", "Hamstrings" },
                    { new Guid("9dce66ea-f645-4e73-85d8-708ae7764469"), "Rope Pushdown", "Triceps" },
                    { new Guid("9e91ccc8-9c9b-45df-82df-11222462ea92"), "Underhand Lat Pulldown", "LatissimusDorsi" },
                    { new Guid("9f43f9e0-eea2-4e75-a6ce-84794701d612"), "Seal Row", "Back" },
                    { new Guid("a75cb8f0-d69c-45b7-8a02-3a97b33708de"), "Reverse Pec Deck", "Shoulders" },
                    { new Guid("b15cb1ac-2b15-440b-806b-7dae01400c7e"), "Machine Chest Press", "Chest" },
                    { new Guid("c075fdf2-f454-4a0c-b847-28124997af36"), "Hip Thrust", "Glutes" },
                    { new Guid("c4b29c8d-5619-4b15-90cc-7d16c46b47d9"), "Ab Wheel Rollout", "Abs" },
                    { new Guid("c9b016c6-9b94-4a7b-b79f-a8c8cd1d409c"), "Arnold Press", "Shoulders" },
                    { new Guid("d0c09849-de38-464d-b482-bff5d3f2df3e"), "Romanian Deadlift", "Hamstrings" },
                    { new Guid("d116cf5f-8cef-47f8-aaac-174575d89491"), "Lat Pulldown", "Back" },
                    { new Guid("d44e141b-8f9a-4c05-8671-39db7a0cbf5b"), "Cable Woodchop", "Obliques" },
                    { new Guid("e73c1ae0-a727-4e9a-a924-674bbea6f355"), "Barbell Curl (Reverse)", "Forearms" },
                    { new Guid("f32be9f2-30d6-4d2a-a870-d222afe081d2"), "Sissy Squat", "Quadriceps" },
                    { new Guid("f39d9923-8e0a-435b-8338-3883e9435f8b"), "Leg Press", "Legs" },
                    { new Guid("f85f449d-713e-41ba-9402-1a517144664a"), "Hamstring Curl", "Legs" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("098c21d8-a07f-4e94-a5c4-cbcab805e57a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("0d9fc89e-d76c-47a4-88b3-9c411c08027c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("1640c229-0e3c-4637-8770-4e1fd2500e65"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("18a106b5-44a0-48dd-94a5-0c548a6f96f0"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("203fa475-9985-47fc-8a5f-deb44a617624"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("208ef74f-77a6-4abe-9e95-c117025861a9"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("33252753-2ddd-4f1b-bb56-555346a70bdc"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("33e0ca24-3b60-40c0-a8c8-793730797ff8"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("355ac64f-15ff-48a0-8b24-ed3e389f341c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("35c6f8bb-5e71-45a5-9ad2-1689edbc20b2"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("3835c651-336b-4196-a1e8-5412ce6a2306"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("384f06df-df2e-4fa7-a19e-9fd40d6cc988"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("3a754d12-8c9d-438e-a79a-3a624e31bbd6"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("3e83a9fe-1cf0-4be0-a110-d4e6ddf22bab"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("40207d57-bbf1-44d8-a540-7a4f2154c41b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("40f7d582-fcc5-4e11-a169-f506e3d71d8e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("42c707f8-277b-4638-bf9c-df54adf2c05d"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("448ea96b-6576-45ae-acf0-45758add7b91"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("4b3d4296-f921-44ef-a770-205443c424cf"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("4b4c5311-8a7d-4197-baec-b37ba3e910a2"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("4efb25c3-3aa4-4430-87bd-184f1fbe2985"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("4f3a65cd-6f34-4c63-bcb1-8c3cd67dc62f"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("52a67749-32e7-40bb-9d8e-e889cd107073"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("54256f9f-ee01-420d-9174-1703d5f25202"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("5edd3cf3-4a78-483f-935e-44123029c2ba"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("6366bd35-fe86-45d0-8563-cf127f4cbbc2"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("6c6985a3-53e3-43a7-b2b7-741cade921d1"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("6e8cd7a9-c344-4a49-b3f3-5ff78dae42e7"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("6f6a9fae-c8fa-4fe7-8fa7-23066bf1eb26"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("6fef3bfc-6b21-4385-9156-361752a8249c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("72de0da4-a19c-427d-906d-caaeab63f49d"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("8a33a91b-9b07-4f97-9de8-15e7006d94ef"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9165606a-bd04-416f-a899-6f509733124b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9343b759-05f6-48a5-8990-4732d0b0e146"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("977edc78-e616-4b53-a82b-0deacf28f264"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("99e68b24-b09a-44d3-8de7-06eec94978de"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9a1582de-8467-4e28-ac6d-d980f1f444c4"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9b2d3052-a6a4-41cf-9e26-8c98244ba3b0"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9dbb1273-74fd-443b-8120-122010d40bfb"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9dce66ea-f645-4e73-85d8-708ae7764469"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9e91ccc8-9c9b-45df-82df-11222462ea92"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("9f43f9e0-eea2-4e75-a6ce-84794701d612"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a75cb8f0-d69c-45b7-8a02-3a97b33708de"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("b15cb1ac-2b15-440b-806b-7dae01400c7e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c075fdf2-f454-4a0c-b847-28124997af36"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c4b29c8d-5619-4b15-90cc-7d16c46b47d9"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("c9b016c6-9b94-4a7b-b79f-a8c8cd1d409c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("d0c09849-de38-464d-b482-bff5d3f2df3e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("d116cf5f-8cef-47f8-aaac-174575d89491"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("d44e141b-8f9a-4c05-8671-39db7a0cbf5b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("e73c1ae0-a727-4e9a-a924-674bbea6f355"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("f32be9f2-30d6-4d2a-a870-d222afe081d2"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("f39d9923-8e0a-435b-8338-3883e9435f8b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("f85f449d-713e-41ba-9402-1a517144664a"));

            migrationBuilder.CreateTable(
                name: "ProgramExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Explanation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Reps = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_ProgramExercises_ExerciseId",
                table: "ProgramExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ProgramId",
                table: "ProgramExercises",
                column: "ProgramId");
        }
    }
}
