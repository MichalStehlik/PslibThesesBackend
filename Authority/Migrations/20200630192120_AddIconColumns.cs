using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authority.Migrations
{
    public partial class AddIconColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "IconImage",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconImageType",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN",
                column: "ConcurrencyStamp",
                value: "561fd2d2-7b13-4e25-8313-8125343da02c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "EXT",
                column: "ConcurrencyStamp",
                value: "5fcbbca6-1d89-4817-9da4-84c5dc1d58e1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "MANAGER",
                column: "ConcurrencyStamp",
                value: "7b971b56-c3d5-4990-bd24-f9191c47418c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "STUDENT",
                column: "ConcurrencyStamp",
                value: "bde6280b-f178-40d1-bf08-8380cc45b108");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "TEACHER",
                column: "ConcurrencyStamp",
                value: "ab78e561-0b5a-4102-9e7c-688cbbde8c67");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMINUSER",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "350450ab-047d-4aac-8bc6-28fe17bea05a", "ST@PSLIB.CLOUD", "ST@PSLIB.CLOUD", "AQAAAAEAACcQAAAAEPpxEh2S83xGGQfSaHD15KSRAHcwuCxsaW4Q8LpWKY+7wL6cgtR+C+pSli6yDUcwDw==", "st@pslib.cloud" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IconImageType",
                table: "AspNetUsers");

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
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "a5311857-faf5-458d-962a-d1a9cde8c711", null, null, "AQAAAAEAACcQAAAAEFuOJWNbL9TT69lpz7nMIr9Ew+PCAtEOyiH0qXsqdv4UsT5yVUja/Ximzgr6RAHfSQ==", "Admin" });
        }
    }
}
