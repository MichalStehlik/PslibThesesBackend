using Microsoft.EntityFrameworkCore.Migrations;

namespace Authority.Migrations
{
    public partial class CorrectionsRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN",
                column: "ConcurrencyStamp",
                value: "1faf9e91-14be-4cbd-a018-5df394db1109");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "EXT",
                column: "ConcurrencyStamp",
                value: "1c8bf5ae-398a-4848-a3c3-7407174a0e26");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "MANAGER",
                column: "ConcurrencyStamp",
                value: "c5befb28-a3c4-46fe-a839-e6df3cf7d0a6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "STUDENT",
                column: "ConcurrencyStamp",
                value: "8f2c36ca-b187-4cd4-96c6-44b1f1105e21");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "TEACHER",
                column: "ConcurrencyStamp",
                value: "8f3b1a16-32cb-4293-b341-61860b3871ce");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMINUSER",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a5311857-faf5-458d-962a-d1a9cde8c711", "AQAAAAEAACcQAAAAEFuOJWNbL9TT69lpz7nMIr9Ew+PCAtEOyiH0qXsqdv4UsT5yVUja/Ximzgr6RAHfSQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN",
                column: "ConcurrencyStamp",
                value: "db54cd75-c099-4f1d-997d-d46e6e69526c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "EXT",
                column: "ConcurrencyStamp",
                value: "2ae52b97-4f3c-4b14-a2fd-a68980e72ea0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "MANAGER",
                column: "ConcurrencyStamp",
                value: "a4b2adee-388b-44b6-a82b-61e889e381e3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "STUDENT",
                column: "ConcurrencyStamp",
                value: "e1e3454d-0a73-4e60-9bb1-c534d3d17490");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "TEACHER",
                column: "ConcurrencyStamp",
                value: "1fca845f-1a10-45ec-a142-0b1b4363178d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMINUSER",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b1fd60d3-c433-4334-8055-f275820d0e33", "AQAAAAEAACcQAAAAEEHOpd0Bili3GcRu+rS17GgO3RZUmqvPLQEfSiLrOkV5EdXWQ+AKA9LGgx5V0StKzQ==" });
        }
    }
}
