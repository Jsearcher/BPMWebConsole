using Microsoft.EntityFrameworkCore.Migrations;

namespace BPMWebConsole.Migrations
{
    public partial class InsertedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5b56ba39-122c-49e0-824b-594508ae0589", "e2b7bb8c-2c77-44bc-8125-8b8832b3b939", "Guest", "GUEST" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f752664f-d326-4ef0-b4bc-b76c056225ff", "b92de15a-b923-48dd-9573-a37a066cca32", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b56ba39-122c-49e0-824b-594508ae0589");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f752664f-d326-4ef0-b4bc-b76c056225ff");
        }
    }
}
